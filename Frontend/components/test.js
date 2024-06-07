import AsyncStorage from "@react-native-async-storage/async-storage";
import axios from "axios";
import React, { useState } from "react";
import { View, Text, Button, Dimensions } from "react-native";
import { ActivityIndicator } from "react-native";
import { Card } from '@rneui/themed';
import Swiper from "react-native-screens-swiper";
import { Platform } from "react-native";
import Home from "./home";
import Test2 from "./test2";


export default function Test()
{
    const data = [
        {
            tabLabel: "Hello World",
            component: Test2,
        },
        {
            tabLabel: "Home",
            component: Home,

        },
    ]


    return (
        <View style={ { flex: 1, height: '25%' } }>
            <View style={ styles.container }>
                <Swiper
                    data={ data }
                    isStaticPills={ true }
                    style={ styles.swiper }
                >

                </Swiper>
            </View>


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
            height: 100,
        },

        pillButton: {
            padding: 40,
        },
        pillActive: {
            backgroundColor: 'darkgrey',
            borderRadius: 5,
        },
        pillLabel: {
            color: Platform.OS === "android" ? "black" : "green",
            height: 40,
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