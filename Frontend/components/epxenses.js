import React, { useEffect, useState } from 'react'
import axios from 'axios'
import AsyncStorage from '@react-native-async-storage/async-storage';
import { FlatList, ScrollView, TouchableOpacity } from 'react-native-gesture-handler';
import { View, Dimensions } from 'react-native';
import { Card } from '@rneui/themed'
import { Text } from 'react-native-elements';
import { useNavigation } from '@react-navigation/native';



export default function Expenses()
{
    const [expenses, setExpenses] = useState([]);


    useEffect(() =>
    {
        fetchExpenses()
    }, [])

    const fetchExpenses = async () => 
    {
        try
        {
            const token = await AsyncStorage.getItem('token');

            if (!token)
            {
                console.error('Token Not Found')
                return
            }

            const response = await axios.get(`http://192.168.0.117:5295/api/Expenses/user/`, {
                headers: {
                    Authorization: `Bearer ${ token }`,
                }
            })
            setExpenses(response.data)
        } catch (error)
        {
            console.error("Failed fetching expenses", error)
        }
    }

    const title = "Spent"
    const money = "$1234"
    const windowWidth = Dimensions.get('window').width;
    const circleSize = windowWidth * 0.3;

    const navigation = useNavigation()
    return (

        <View style={ styles.container }>
            <ScrollView style={ styles.scroll }>
                <View style={ styles.circleContainer }>
                    <View style={ [styles.circle, { width: circleSize, height: circleSize, borderRadius: circleSize / 2, backgroundColor: '#ddd' }] }>
                        <Text style={ { fontSize: 18, marginBottom: 10 } }>Spent</Text>
                        <Text style={ { fontSize: 30, fontWeight: 'bold' } }>$122</Text>
                    </View>
                </View>

                <View style={ { backgroundColor: '#eee', margin: 0 } }>
                </View>

                <View style={ { margin: 0, backgroundColor: 'green' } }>
                    <Text h4>Transactions</Text>
                    <FlatList
                        data={ expenses }
                        renderItem={ ({ item }) => (
                            <Card containerStyle={ { borderRadius: 10, margin: 10 } }>
                                <View style={ { flexDirection: 'row', justifyContent: 'space-between' } }>
                                    <Text>{ item.name }</Text>
                                    <Text>{ item.amount }</Text>
                                </View>
                            </Card>
                        ) }
                    />
                </View>

            </ScrollView>

            <TouchableOpacity
                style={ styles.addButton }
                onPress={ () =>
                {
                    navigation.navigate('AddExpense')
                } }
            >
                <Text style={ { color: 'white', fontSize: 24 } }>+</Text>
            </TouchableOpacity>

        </View >
    )
}


const styles = {
    scroll: {
        maxHeight: '99.9%',
    },
    container: {
        maxHeight: '100%',
    },
    circleContainer: {
        width: '100%',
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: 'lightblue',
        padding: 5,
        height: 150,
    },
    circle: {
        justifyContent: 'center',
        alignItems: 'center',
    },
    addButton: {
        position: 'absolute',
        bottom: 5,
        right: 172,
        backgroundColor: 'blue',
        width: 60,
        height: 50,
        borderRadius: 30,
        justifyContent: 'center',
        alignItems: 'center',
    }
};