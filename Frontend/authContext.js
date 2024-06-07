import AsyncStorage from '@react-native-async-storage/async-storage';
import React, { Children, createContext, useContext, useEffect, useState, } from 'react';
import axios from 'axios';
import { Alert } from 'react-native';


export const AuthContext = createContext()

export const AuthProvider = ({ children }) =>
{

    const [token, setToken] = useState(null)

    const handleLogin = async (username, password) =>
    {
        try
        {
            const response = await axios.post(`http://192.168.0.117:5295/api/auth/login`, {
                username,
                password
            }, {
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            if (!response.status === 200)
            {
                throw new Error('Invalid credentials');
            }
            await AsyncStorage.setItem('token', response.data.token)
            const token = await AsyncStorage.getItem('token')
            setToken(token)

        }
        catch (error)
        {
            console.error('Login Failed:', error);
            Alert.alert('Login Failed', 'Invalid username or password.')
        }

    }

    const isLoggedIn = async () =>
    {
        try
        {
            const token = await AsyncStorage.getItem('token')
            if (token != null)
            {
                setToken(token)
            } else
            {
                return
            }

        } catch (error)
        {
            console.error('isLoggedIn Error', error)
        }
    }

    const logout = async () =>
    {
        try
        {
            await AsyncStorage.removeItem('token')
            setToken(null)
            axios.defaults.headers.common['Authorization'] = ``
        } catch (error)
        {
            console.error('Logout failed', error)
        }
    }
    useEffect(() =>
    {
        isLoggedIn()
    }, [])


    return (
        <AuthContext.Provider value={ { handleLogin, token, logout } }>
            { children }
        </AuthContext.Provider>
    )
}

