import React, { useContext } from 'react';
import { createDrawerNavigator } from '@react-navigation/drawer';
import { View, Button, StyleSheet, Image, SafeAreaView, ScrollView } from 'react-native';
import Dashboard from './Dashboard';
import SettingsScreen from './SettingsScreen';
import { AuthContext } from '../authContext';
import { DrawerItemList } from '@react-navigation/drawer';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

const Drawer = createDrawerNavigator();

export default function MenuDrawer()
{
    const { logout } = useContext(AuthContext);
    const insets = useSafeAreaInsets();

    const CustomDrawerContent = (props) => (
        <SafeAreaView style={ { flex: 1, paddingTop: insets.top } }>
            <ScrollView contentContainerStyle={ { flex: 1 } }>
                <DrawerItemList { ...props } />

                <View style={ styles.logoutButtonContainer }>
                    <Button onPress={ logout } title="Logout" color="#00cc00" />
                </View>
            </ScrollView>
        </SafeAreaView>
    );

    return (
        <Drawer.Navigator
            screenOptions={ {
                drawerPosition: 'right',
                headerShown: false,
                overlayColor: 'rgba(0, 0, 0, 0.7)',
            } }
            drawerContent={ (props) => <CustomDrawerContent { ...props } /> }
        >
            <Drawer.Screen
                name="Dashboard"
                component={ Dashboard }
                options={ {
                    drawerLabel: 'Home',
                    drawerIcon: () => (
                        <Image
                            source={ require('../icons/HomeIcon.png') }
                            style={ { width: 30, height: 30 } }
                        />
                    ),
                } }
            />

            <Drawer.Screen
                name="Settings"
                component={ SettingsScreen }
                options={ {
                    drawerLabel: 'Settings',
                    drawerIcon: () => (
                        <Image
                            source={ require('../icons/SettingsIcon.png') }
                            style={ { width: 30, height: 30 } }
                        />
                    ),
                } }
            />
        </Drawer.Navigator>
    );
};

const styles = StyleSheet.create({
    logoutButtonContainer: {
        marginTop: 'auto',
        paddingBottom: 20,
    },
    drawerItems: {
        flex: 1,
    },
});
