import AsyncStorage from '@react-native-async-storage/async-storage';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, Alert } from 'react-native';
import { FlatList } from 'react-native-gesture-handler';
import { Card } from '@rneui/themed';
import { useNavigation } from '@react-navigation/native';

export default function Subscriptions()
{
    const [subscriptions, setSubscriptions] = useState([]);
    const navigation = useNavigation();

    useEffect(() =>
    {
        fetchSubscriptions();
    }, []);

    const fetchSubscriptions = async () =>
    {
        try
        {
            const token = await AsyncStorage.getItem('token');
            const response = await axios.get(`http://192.168.0.2:5295/api/Subscriptions/user/`, {
                headers: {
                    Authorization: `Bearer ${ token }`,
                },
            });
            setSubscriptions(response.data);
        } catch (error)
        {
            console.error('Failed fetching subscriptions: ', error);
        }
    };

    const handleRowPress = (item) =>
    {
        Alert.alert("Subscription Details", `Name: ${ item.name }\nAmount: €${ item.amount }\nNext Billing Date: ${ item.nextBillingDate }`);
    };

    const renderItem = ({ item }) => (
        <TouchableOpacity onPress={ () => handleRowPress(item) }>
            <Card containerStyle={ styles.card }>
                <View style={ styles.cardRow }>
                    <Text style={ styles.subscriptionName }>{ item.name }</Text>
                    <Text style={ styles.subscriptionAmount }>€{ item.amount }</Text>
                </View>
            </Card>
        </TouchableOpacity>
    );

    const renderHeader = () => (
        <TouchableOpacity style={ styles.addButton } onPress={ () => navigation.navigate('AddSubscription') }>
            <Text style={ styles.addButtonText }>+ Add New Subscription</Text>
        </TouchableOpacity>
    );

    return (
        <View style={ styles.container }>
            <View style={ styles.header }>
                <Text style={ styles.headerTitle }>Subscriptions</Text>
            </View>
            <FlatList
                data={ subscriptions }
                renderItem={ renderItem }
                keyExtractor={ (item) => item.id || item.name }
                contentContainerStyle={ styles.listContainer }
                ListHeaderComponent={ renderHeader }
            />
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#ffffff',
        padding: 10,
    },
    header: {
        alignItems: 'center',
        marginBottom: 20,
    },
    headerTitle: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#1E90FF',
    },
    listContainer: {
        paddingBottom: 20,
    },
    card: {
        backgroundColor: '#f5f5f5',
        borderRadius: 10,
        padding: 15,
        marginBottom: 10,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.1,
        shadowRadius: 2,
    },
    cardRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    subscriptionName: {
        fontSize: 16,
        fontWeight: 'bold',
        color: '#333333',
    },
    subscriptionAmount: {
        fontSize: 14,
        color: '#333333',
    },
    addButton: {
        backgroundColor: '#1E90FF',
        borderRadius: 10,
        padding: 15,
        marginBottom: 20,
        justifyContent: 'center',
        alignItems: 'center',
    },
    addButtonText: {
        fontSize: 16,
        color: 'white',
        fontWeight: 'bold',
    },
});
