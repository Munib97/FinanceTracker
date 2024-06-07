import AsyncStorage from "@react-native-async-storage/async-storage";
import axios from "axios";
import React, { useState } from "react";
import { View, Text, Button, Dimensions } from "react-native";
import { ActivityIndicator } from "react-native";

export default function Test2()
{

    const title = "Spent"
    const money = "$1234"
    const windowWidth = Dimensions.get('window').width;

    const circleSize = windowWidth * 0.5;


    return (
        <View style={ {
            width: '100%',
            justifyContent: 'center',
            alignItems: 'center',
            backgroundColor: 'lightblue',
            padding: 5,
        } }>
            <View style={ {
                width: circleSize,
                height: circleSize,
                borderRadius: circleSize / 2,
                backgroundColor: '#ddd',
                justifyContent: 'center',
                alignItems: 'center',
            } }>
                <Text style={ { fontSize: 18, marginBottom: 10 } }>Spent</Text>
                <Text style={ { fontSize: 30, fontWeight: 'bold' } }>$122</Text>
            </View>
        </View>
    );
}
