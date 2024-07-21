import React, { useState, useEffect, useContext } from "react";
import Swiper from "react-native-screens-swiper";
import { Platform } from "react-native";
import { View, Text, Button } from "react-native";
import Expenses from "./epxenses";
import Home from "./home";
import Subscriptions from "./subscriptions";
import { AuthContext } from "../authContext";





export default function Dashboard()
{

    const { logout } = useContext(AuthContext)


    const data = [
        {
            tabLabel: "Expenses",
            component: Expenses,
        },
        {
            tabLabel: "Dashboard",
            component: Home,
        },
        {
            tabLabel: "Subscriptions",
            component: Subscriptions
        }
    ]



    return (

        <View style={ styles.container }>
            <Button
                title="logout"
                onPress={ logout }
            />
            <Swiper
                data={ data }
                isStaticPills={ true }
                style={ styles.swiper }
            >
            </Swiper>
        </View>
    );
}

const styles = {
    container: {
        flex: 1,
        justifyContent: "flex-end",
    },
    swiper: {
        pillContainer: {
            backgroundColor: Platform.OS === "android" ? "lightgrey" : "darkgrey",
            alignItems: 'center',
            height: 40,

        },

        pillButton: {
            padding: 18,
        },
        pillActive: {
            backgroundColor: 'darkgrey',
            borderRadius: 5,
        },
        pillLabel: {
            color: Platform.OS === "android" ? "black" : "green",
            height: 20,
        },
        activeLabel: {
            color: 'red',
        },
        borderActive: {
            borderColor: 'black',
            borderBottomWidth: 2,
        },
    }
};