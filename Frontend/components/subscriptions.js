import AsyncStorage from '@react-native-async-storage/async-storage'
import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { View, Text, TouchableOpacity } from 'react-native'
import { FlatList } from 'react-native-gesture-handler'
import { useNavigation } from '@react-navigation/native';




export default function Subscriptions()
{

    const [subscriptions, setSubscriptions] = useState([])



    useEffect(() =>
    {
        fetchSubscriptions()
    }, [])


    const fetchSubscriptions = async () =>
    {
        try
        {
            const token = await AsyncStorage.getItem('token')
            const response = await axios.get(`http://192.168.0.117:5295/api/Subscriptions/user/`, {
                headers: {
                    Authorization: `Bearer ${ token }`
                }
            })
            setSubscriptions(response.data)
        } catch (error)
        {
            console.error("Failed fetching subscriptions: ", error)
        }

    }


    const navigation = useNavigation()
    return (
        <View>
            <FlatList
                style={ styles.list }
                data={ subscriptions }
                renderItem={ ({ item }) => (
                    <View style={ styles.item }>
                        <Text>{ item.name }</Text>

                        <Text>{ item.amount }</Text>

                    </View>
                )
                }
            />
            <TouchableOpacity
                style={ styles.addButton }
                onPress={ () =>
                {
                    navigation.navigate('AddSubscription')
                } }
            >
                <Text style={ { color: 'white', fontSize: 24 } }>+</Text>
            </TouchableOpacity>
        </View>
    )
}

const styles = {
    list: {
        backgroundColor: 'lightgrey',
        height: '100%',
    },
    item: {
        borderWidth: 1,
        margin: 1,
        backgroundColor: 'grey',
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
}