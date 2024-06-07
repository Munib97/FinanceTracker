import React, { useEffect, useState } from 'react'
import { View, Text, Alert, TextInput, Button, StyleSheet, Modal } from 'react-native'
import axios from 'axios'
import DateTimePicker from '@react-native-community/datetimepicker'
import { TouchableOpacity } from 'react-native-gesture-handler'
import { Dropdown } from 'react-native-element-dropdown'
import AsyncStorage from '@react-native-async-storage/async-storage'

export default function AddExpense()
{
    const [amount, setAmount] = useState('')
    const [name, setName] = useState('')
    const [dateSpent, setDateSpent] = useState(new Date())
    const [showDatePicker, setShowDatePicker] = useState(false)
    const [categories, setCategories] = useState([])
    const [selectedValue, setSelectedValue] = useState(null)

    useEffect(() =>
    {
        getCategories()
    }, [])

    const getCategories = async () =>
    {
        const token = AsyncStorage.getItem('token');
        const response = await axios.get(`http://192.168.0.117:5295/api/Categories`, {
            headers: {
                Authorization: `Bearer ${ token }`
            }
        })

        setCategories(response.data)
    }

    const handleSubmit = async () =>
    {
        try
        {
            const token = AsyncStorage.getItem('token');
            const formattedDate = dateSpent.toISOString()
            const response = await axios.post(`http://192.168.0.117:5295/api/expenses`, {

                amount: parseFloat(amount),
                name: name,
                dateSpent: formattedDate,
                categoryId: selectedValue,
            },
                {
                    headers: {
                        Authorization: `Bearer ${ token }`
                    }
                }
            )
            Alert.alert('Success', 'Expense created successfully')
            setAmount('')
            setName('')
            setDateSpent(new Date())
            setSelectedValue(null)
        } catch (error)
        {
            console.error('Error creating expense: ', error)
            Alert.alert('Error', 'Failed to create expense')
        }
    }

    const handleDateChange = (event, selectedDate) =>
    {
        const currentDate = selectedDate || dateSpent;
        setShowDatePicker(false);
        setDateSpent(currentDate);
    }

    const showDatePickerModal = () =>
    {
        setShowDatePicker(true)
    }

    return (
        <View style={ styles.container }>
            <View style={ styles.inputContainer }>
                <Text style={ styles.label }>Expense Name</Text>
                <TextInput
                    style={ styles.input }
                    placeholder="Expense Name"
                    onChangeText={ text => setName(text) }
                    value={ name }
                />
            </View>

            <View style={ styles.inputContainer }>
                <Text style={ styles.label }>Price</Text>
                <TextInput
                    style={ styles.input }
                    placeholder="Amount"
                    onChangeText={ text => setAmount(text) }
                    value={ amount }
                    keyboardType="numeric"
                />
            </View>

            <TouchableOpacity
                style={ styles.inputContainer }
                onPress={ showDatePickerModal }
            >
                <Text style={ styles.label }>Date</Text>
                <View style={ styles.datePickerContainer }>
                    <Text style={ styles.dateText }>{ dateSpent.toLocaleDateString('en-US', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric',
                    }) }</Text>
                    { showDatePicker && (
                        <DateTimePicker
                            value={ dateSpent }
                            mode="date"
                            display="spinner"
                            onChange={ handleDateChange }
                        />
                    ) }
                </View>
            </TouchableOpacity>

            <View style={ styles.inputContainer }>
                <Dropdown
                    data={ categories }
                    labelField="name"
                    valueField="categoryId"
                    mode='default'
                    style={ styles.dropdown }
                    placeholder='Select a Category'
                    value={ selectedValue }
                    onChange={ (item) => setSelectedValue(item?.categoryId) }
                />

            </View>

            <Button title="Create" onPress={ handleSubmit } />
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    inputContainer: {
        marginBottom: 10,
        width: '80%',
    },
    label: {
        marginBottom: 5,
        fontSize: 16,
    },
    input: {
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 5,
        padding: 10,
        width: '100%',
    },
    datePickerContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        width: '100%'
    },
    dateText: {
        flex: 1,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 5,
        padding: 10,
        marginRight: 10,
    },
    dropdown: {
        width: 200
    }
});
