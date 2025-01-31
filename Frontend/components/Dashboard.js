import React, { useContext } from 'react';
import { View, Button, StyleSheet, Image, TouchableOpacity, SafeAreaView } from 'react-native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createDrawerNavigator } from '@react-navigation/drawer';
import Expenses from './expenses';
import Home from './home';
import Subscriptions from './subscriptions';
import ExpensesIcon from '../icons/ExpensesIcon.png';
import HomeIcon from '../icons/HomeIcon.png';
import SubscriptionsIcon from '../icons/SubscriptionsIcon.png';
import MenuIcon from '../icons/MenuIcon.png';

const Tab = createBottomTabNavigator();
const Drawer = createDrawerNavigator();


const Dashboard = ({ navigation }) =>
{
    return (
        <SafeAreaView style={ { flex: 1 } }>
            <Tab.Navigator
                screenOptions={ ({ route }) => ({
                    tabBarIcon: ({ focused, color, size }) =>
                    {
                        let iconSource;
                        if (route.name === 'Expenses')
                        {
                            iconSource = ExpensesIcon;
                        } else if (route.name === 'Home')
                        {
                            iconSource = HomeIcon;
                        } else if (route.name === 'Subscriptions')
                        {
                            iconSource = SubscriptionsIcon;
                        }
                        return (
                            <Image
                                source={ iconSource }
                                style={ [styles.tabIcon, { tintColor: color }] }
                                resizeMode="contain"
                            />
                        );
                    },
                    tabBarActiveTintColor: 'darkgrey',
                    tabBarInactiveTintColor: 'lightgrey',
                }) }
            >
                <Tab.Screen name="Expenses" component={ Expenses } />
                <Tab.Screen name="Home" component={ Home } />
                <Tab.Screen name="Subscriptions" component={ Subscriptions } />
            </Tab.Navigator>


            <TouchableOpacity
                style={ styles.hamburgerButton }
                onPress={ () => navigation.toggleDrawer() }
            >
                <Image
                    source={ MenuIcon }
                    style={ { width: 30, height: 30 } }
                />
            </TouchableOpacity>
        </SafeAreaView>
    );
};

const styles = StyleSheet.create({
    tabIcon: {
        width: 30,
        height: 30,
    },
    hamburgerButton: {
        position: 'absolute',
        top: 40,
        right: 10,
        zIndex: 1,
        padding: 10,
    },
});

export default Dashboard;
