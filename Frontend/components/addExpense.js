import React, { useEffect, useState } from 'react';
import { View, Text, Alert, TextInput, Button, StyleSheet, Modal } from 'react-native';
import axios from 'axios';
import DateTimePicker from '@react-native-community/datetimepicker';
import { TouchableOpacity } from 'react-native-gesture-handler';
import { Dropdown } from 'react-native-element-dropdown';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function AddExpense()
{
    const [amount, setAmount] = useState('');
    const [name, setName] = useState('');
    const [dateSpent, setDateSpent] = useState(new Date());
    const [showDatePicker, setShowDatePicker] = useState(false);
    const [categories, setCategories] = useState([]);
    const [selectedValue, setSelectedValue] = useState(null);

    useEffect(() =>
    {
        getCategories();
    }, []);

    const getCategories = async () =>
    {
        const token = await AsyncStorage.getItem('token');
        const response = await axios.get(`http://192.168.0.117:5295/api/Categories`, {
            headers: {
                Authorization: `Bearer ${ token }`,
            },
        });

        setCategories(response.data);
    };

    const handleSubmit = async () =>
    {
        try
        {
            const token = await AsyncStorage.getItem('token');
            const formattedDate = dateSpent.toISOString();
            await axios.post(
                `http://192.168.0.117:5295/api/expenses`,
                {
                    amount: parseFloat(amount),
                    name: name,
                    dateSpent: formattedDate,
                    categoryId: selectedValue,
                },
                {
                    headers: {
                        Authorization: `Bearer ${ token }`,
                    },
                }
            );
            Alert.alert('Success', 'Expense created successfully');
            setAmount('');
            setName('');
            setDateSpent(new Date());
            setSelectedValue(null);
        } catch (error)
        {
            console.error('Error creating expense: ', error);
            Alert.alert('Error', 'Failed to create expense');
        }
    };

    const handleDateChange = (event, selectedDate) =>
    {
        const currentDate = selectedDate || dateSpent;
        setShowDatePicker(false);
        setDateSpent(currentDate);
    };

    const showDatePickerModal = () =>
    {
        setShowDatePicker(true);
    };

    return (
        <View style={ styles.container }>
            <View style={ styles.header }>
                <Text style={ styles.headerTitle }>Add New Expense</Text>
            </View>

            <View style={ styles.form }>
                <View style={ styles.inputContainer }>
                    <Text style={ styles.label }>Expense Name</Text>
                    <TextInput
                        style={ styles.input }
                        placeholder="Expense Name"
                        onChangeText={ (text) => setName(text) }
                        value={ name }
                    />
                </View>

                <View style={ styles.inputContainer }>
                    <Text style={ styles.label }>Amount</Text>
                    <TextInput
                        style={ styles.input }
                        placeholder="Amount"
                        onChangeText={ (text) => setAmount(text) }
                        value={ amount }
                        keyboardType="numeric"
                    />
                </View>

                <TouchableOpacity style={ styles.inputContainer } onPress={ showDatePickerModal }>
                    <Text style={ styles.label }>Date</Text>
                    <View style={ styles.datePickerContainer }>
                        <Text style={ styles.dateText }>
                            { dateSpent.toLocaleDateString('en-US', {
                                weekday: 'long',
                                year: 'numeric',
                                month: 'long',
                                day: 'numeric',
                            }) }
                        </Text>
                        { showDatePicker && (
                            <DateTimePicker value={ dateSpent } mode="date" display="spinner" onChange={ handleDateChange } />
                        ) }
                    </View>
                </TouchableOpacity>

                <View style={ styles.inputContainer }>
                    <Text style={ styles.label }>Category</Text>
                    <Dropdown
                        data={ categories }
                        labelField="name"
                        valueField="categoryId"
                        mode="default"
                        style={ styles.dropdown }
                        placeholder="Select a Category"
                        value={ selectedValue }
                        onChange={ (item) => setSelectedValue(item?.categoryId) }
                    />
                </View>

                <TouchableOpacity style={ styles.createButton } onPress={ handleSubmit }>
                    <Text style={ styles.createButtonText }>Create Expense</Text>
                </TouchableOpacity>
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#ffffff',
        padding: 20,
    },
    header: {
        marginBottom: 20,
        alignItems: 'center',
    },
    headerTitle: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#1E90FF',
    },
    form: {
        width: '100%',
        justifyContent: 'center',
        alignItems: 'center',
    },
    inputContainer: {
        marginBottom: 20,
        width: '100%',
    },
    label: {
        marginBottom: 5,
        fontSize: 16,
        color: '#1E90FF',
    },
    input: {
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 10,
        padding: 10,
        width: '100%',
        backgroundColor: '#f5f5f5',
    },
    datePickerContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        width: '100%',
    },
    dateText: {
        flex: 1,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 10,
        padding: 10,
        backgroundColor: '#f5f5f5',
    },
    dropdown: {
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 10,
        padding: 10,
        width: '100%',
        backgroundColor: '#f5f5f5',
    },
    createButton: {
        backgroundColor: '#1E90FF',
        borderRadius: 10,
        padding: 15,
        justifyContent: 'center',
        alignItems: 'center',
        width: '100%',
    },
    createButtonText: {
        fontSize: 16,
        color: 'white',
        fontWeight: 'bold',
    },
});
