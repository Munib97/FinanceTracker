import axios from "axios";
import { FontAwesome } from "@expo/vector-icons";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { AuthContext } from "../authContext";
import React, { useContext, useState } from "react";
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert } from 'react-native';
import { Button, Input } from 'react-native-elements';
import { useNavigation } from "@react-navigation/native";

export default function Register()
{
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [confirmPassword, setConfirmPassword] = useState('')
    const [email, setEmail] = useState('')
    const navigation = useNavigation('')

    const handleSubmit = async () =>
    {
        if (password !== confirmPassword)
        {
            Alert.alert("Passwords do not match");
            return;
        }
        try
        {
            const response = await axios.post(`http://192.168.0.117:5295/api/auth/register`, {
                username,
                password,
                email,
            },
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            )
            Alert.alert('Success', 'Account created successfully')
            setUsername('')
            setPassword('')
            setEmail('')
        } catch (error)
        {
            console.error('Error creating account: ', error)
            Alert.alert('Error', 'Failed to create account')
        }
    }

    return (
        <View style={ styles.container }>
            <Text style={ styles.title }>Create new account</Text>

            <Input
                placeholder="Email"
                leftIcon={
                    <FontAwesome name="envelope" size={ 24 } color="#000" />
                }
                onChangeText={ setEmail }
                value={ email }
                containerStyle={ styles.inputContainer }
            />
            <Input
                placeholder="Choose a username"
                leftIcon={
                    <FontAwesome name="user" size={ 24 } color="#000" />
                }
                onChangeText={ setUsername }
                value={ username }
                containerStyle={ styles.inputContainer }
            />
            <Input
                placeholder="Set Password"
                leftIcon={
                    <FontAwesome name="lock" size={ 24 } color="#000" />
                }
                onChangeText={ setPassword }
                value={ password }
                secureTextEntry={ true }
                containerStyle={ styles.inputContainer }
            />
            <Input
                placeholder="Confirm password"
                leftIcon={
                    <FontAwesome name="lock" size={ 24 } color="#000" />
                }
                onChangeText={ setConfirmPassword }
                value={ confirmPassword }
                secureTextEntry={ true }
                containerStyle={ styles.inputContainer }
            />
            <Button
                title="Login"
                buttonStyle={ styles.loginButton }
                onPress={ () => handleSubmit(username, password, email) }
            />
            <TouchableOpacity style={ styles.forgotPassword } onPress={ () => navigation.navigate("LoginScreen") }>
                <Text style={ styles.createAccountText }>Already have an account?</Text>
            </TouchableOpacity>
        </View >
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#f5f5f5',
        padding: 20,
    },
    title: {
        fontSize: 32,
        fontWeight: 'bold',
        marginBottom: 20,
    },
    inputContainer: {
        width: '100%',
        marginBottom: 10,
    },
    loginButton: {
        backgroundColor: '#4CAF50',
        borderRadius: 5,
        padding: 15,
        width: '100%',
    },
    forgotPassword: {
        marginTop: 15,
    },
    forgotPasswordText: {
        color: '#000',
        fontSize: 16,
    },
    createAccountText: {
        color: '#0000FF',
    },
});
