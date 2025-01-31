import React, { useContext } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { AuthContext } from './authContext';
import MenuDrawer from './components/MenuDrawer';
import AddExpense from './components/addExpense';
import AddSubscription from './components/addSubscription';
import LoginScreen from './components/LoginScreen';
import Register from './components/Register';

const Stack = createStackNavigator();

export default function AppNav()
{
    const { token, logout } = useContext(AuthContext);

    return (
        <NavigationContainer>
            { token == null ? (
                <Stack.Navigator>
                    <Stack.Screen name="LoginScreen" component={ LoginScreen } />
                    <Stack.Screen name="Register" component={ Register } />
                </Stack.Navigator>
            ) : (
                <Stack.Navigator>
                    <Stack.Screen
                        name="Expense Tracker"
                        component={ MenuDrawer }
                        options={ { headerShown: false } }
                    />
                    <Stack.Screen name="AddExpense" component={ AddExpense } />
                    <Stack.Screen name="AddSubscription" component={ AddSubscription } />
                </Stack.Navigator>
            ) }
        </NavigationContainer>
    );
};

