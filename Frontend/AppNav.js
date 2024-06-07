import React, { useState, useEffect, useContext } from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { NavigationContainer } from '@react-navigation/native'
import { createStackNavigator } from '@react-navigation/stack'
import Home from './components/home';
import Dashboard from './components/Dashboard'
import Expenses from './components/epxenses';
import Subscriptions from './components/subscriptions';
import AddExpense from './components/addExpense';
import AddSubscription from './components/addSubscription';
import { AuthContext } from './authContext';
import LoginScreen from './components/LoginScreen';
import Register from './components/Register';


const Stack = createStackNavigator()

export const AppNav = () =>
{
    const { token } = useContext(AuthContext)
    return (
        <NavigationContainer>

            { token == null ? (
                <Stack.Navigator>
                    <Stack.Screen name='LoginScreen' component={ LoginScreen } />
                    <Stack.Screen name='Register' component={ Register } />
                </Stack.Navigator>
            ) : (
                <>
                    <Stack.Navigator>
                        <Stack.Screen name='Dashboard' component={ Dashboard } />
                        <Stack.Screen name='Home' component={ Home } />
                        <Stack.Screen name='Expenses' component={ Expenses } />
                        <Stack.Screen name='Subscriptions' component={ Subscriptions } />
                        <Stack.Group screenOptions={ { presentation: 'modal' } }>
                            <Stack.Screen name='AddExpense' component={ AddExpense } />
                            <Stack.Screen name='AddSubscription' component={ AddSubscription } />
                        </Stack.Group>
                    </Stack.Navigator>
                </>
            ) }

        </NavigationContainer>
    )
}

export default AppNav