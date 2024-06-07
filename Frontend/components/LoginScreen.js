import axios from "axios";
import { FontAwesome } from "@expo/vector-icons";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { AuthContext } from "../authContext";
import React, { useContext, useState } from "react";
import { View, Text, TextInput, TouchableOpacity, StyleSheet } from 'react-native';
import { Button, Input } from 'react-native-elements';
import { useNavigation } from "@react-navigation/native";

const LoginScreen = () =>
{
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const { handleLogin } = useContext(AuthContext);
    const navigation = useNavigation();

    return (
        <View style={ styles.container }>
            <Text style={ styles.title }>Welcome</Text>

            <Input
                placeholder="Username"
                leftIcon={
                    <FontAwesome name="user" size={ 24 } color="#000" />
                }
                onChangeText={ setUsername }
                value={ username }
                containerStyle={ styles.inputContainer }
            />
            <Input
                placeholder="Password"
                leftIcon={
                    <FontAwesome name="lock" size={ 24 } color="#000" />
                }
                onChangeText={ setPassword }
                value={ password }
                secureTextEntry={ true }
                containerStyle={ styles.inputContainer }
            />
            <Button
                title="Login"
                buttonStyle={ styles.loginButton }
                onPress={ () => handleLogin(username, password) }
            />
            <TouchableOpacity style={ styles.forgotPassword }>
                <Text style={ styles.forgotPasswordText }>Forgot Password?</Text>
            </TouchableOpacity>
            <TouchableOpacity style={ styles.forgotPassword } onPress={ () => navigation.navigate("Register") }>
                <Text style={ styles.createAccountText }>Create Account</Text>
            </TouchableOpacity>
        </View>
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

export default LoginScreen;
