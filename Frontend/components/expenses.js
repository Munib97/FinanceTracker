import React, { useEffect, useState } from 'react';
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { View, Text, FlatList, TouchableOpacity, StyleSheet } from 'react-native';
import { Card } from '@rneui/themed';
import { useNavigation } from '@react-navigation/native';

export default function Expenses()
{
    const [expenses, setExpenses] = useState([]);

    useEffect(() =>
    {
        fetchExpenses();
    }, []);

    const fetchExpenses = async () =>
    {
        try
        {
            const token = await AsyncStorage.getItem('token');
            if (!token)
            {
                console.error('Token Not Found');
                return;
            }

            const response = await axios.get(`http://192.168.0.2:5295/api/Expenses/user/`, {
                headers: {
                    Authorization: `Bearer ${ token }`,
                },
            });
            setExpenses(response.data);
        } catch (error)
        {
            console.error("Failed fetching expenses", error);
        }
    };

    const navigation = useNavigation();

    const renderItem = ({ item }) => (
        <Card containerStyle={ styles.card }>
            <View style={ styles.cardRow }>
                <Text style={ styles.expenseName }>{ item.name }</Text>
                <Text style={ styles.expenseAmount }>{ item.amount }</Text>
            </View>
        </Card>
    );

    const renderHeader = () => (
        <TouchableOpacity style={ styles.addButton } onPress={ () => navigation.navigate('AddExpense') }>
            <Text style={ styles.addButtonText }>+ Add New Expense</Text>
        </TouchableOpacity>
    );

    return (
        <View style={ styles.container }>
            <View style={ styles.header }>
                <Text style={ styles.headerTitle }>Spent</Text>
                <Text style={ styles.totalAmount }>$1234</Text>
            </View>
            <FlatList
                data={ expenses }
                renderItem={ renderItem }
                keyExtractor={ (item) => item.id }
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
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        marginBottom: 20,
    },
    headerTitle: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#1E90FF',
    },
    totalAmount: {
        fontSize: 20,
        color: '#333333',
    },
    listContainer: {
        paddingBottom: 20,
    },
    card: {
        backgroundColor: '#f5f5f5',
        borderRadius: 10,
        padding: 15,
        marginBottom: -3,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.1,
        shadowRadius: 2,
    },
    cardRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    expenseName: {
        fontSize: 16,
        fontWeight: 'bold',
        color: '#333333',
    },
    expenseAmount: {
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
