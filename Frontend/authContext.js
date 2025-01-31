import AsyncStorage from '@react-native-async-storage/async-storage';
import React, { createContext, useContext, useEffect, useState } from 'react';
import axios from 'axios';
import { Alert } from 'react-native';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) =>
{
    const [token, setToken] = useState(null);

    const handleLogin = async (username, password) =>
    {
        try
        {
            const response = await axios.post(`http://192.168.0.2:5295/api/auth/login`, {
                username,
                password
            }, {
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            if (response.status !== 200)
            {
                throw new Error('Invalid credentials');
            }
            await AsyncStorage.setItem('token', response.data.token);
            setToken(response.data.token);
        } catch (error)
        {
            console.error('Login Failed:', error);
            Alert.alert('Login Failed', 'Invalid username or password.');
        }
    };

    const validateToken = async (storedToken) =>
    {
        try
        {
            const response = await axios.get(`http://192.168.0.2:5295/api/auth/validateToken`, {
                headers: {
                    'Authorization': `Bearer ${ storedToken }`
                }
            });
            return response.status === 200;
        } catch (error)
        {
            console.error('Token validation failed:', error);
            return false;
        }
    };

    const isLoggedIn = async () =>
    {
        try
        {
            const storedToken = await AsyncStorage.getItem('token');
            if (storedToken)
            {
                const isValid = await validateToken(storedToken);
                if (isValid)
                {
                    setToken(storedToken);
                } else
                {
                    await AsyncStorage.removeItem('token');
                    setToken(null);
                }
            }
        } catch (error)
        {
            console.error('isLoggedIn Error', error);
        }
    };

    const logout = async () =>
    {
        try
        {
            await AsyncStorage.removeItem('token');
            setToken(null);
            axios.defaults.headers.common['Authorization'] = ``;
        } catch (error)
        {
            console.error('Logout failed', error);
        }
    };

    useEffect(() =>
    {
        isLoggedIn();
    }, []);

    return (
        <AuthContext.Provider value={ { handleLogin, token, logout } }>
            { children }
        </AuthContext.Provider>
    );
};
