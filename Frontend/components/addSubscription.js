import React, { useEffect, useState } from 'react';
import { View, Text, Alert, TextInput, Button, StyleSheet, Modal } from 'react-native';
import axios from 'axios';
import DateTimePicker from '@react-native-community/datetimepicker';
import { TouchableOpacity } from 'react-native-gesture-handler';
import { Dropdown } from 'react-native-element-dropdown';
import { sub } from 'date-fns';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function AddSubscription()
{

    const [name, setName] = useState('')
    const [amount, setAmount] = useState('')
    const [frequency, setFrequency] = useState(null)
    const [startDate, setStartDate] = useState(new Date())
    const [endDate, setEndDate] = useState(new Date())
    const [categories, setCategories] = useState([])
    const [selectedFrequencyValue, setSelectedFrequencyValue] = useState(null)
    const [showStartDatePicker, setShowStartDatePicker] = useState(false)
    const [showEndDatePicker, setShowEndDatePicker] = useState(false)
    const [token, setToken] = useState('')
    const [selectedCategoryValue, setSelectedCategoryValue] = useState(null)

    const SubscriptionFrequency = {
        Weekly: 'Weekly',
        Monthly: 'Monthly',
        Quarterly: 'Quarterly',
        Biannually: 'Biannually',
        Yearly: 'Yearly'
    };

    const frequencyIndexMap = {
        [SubscriptionFrequency.Weekly]: 0,
        [SubscriptionFrequency.Monthly]: 1,
        [SubscriptionFrequency.Quarterly]: 2,
        [SubscriptionFrequency.Biannually]: 3,
        [SubscriptionFrequency.Yearly]: 4,
    };
    useEffect(() =>
    {

        getCategories();

    }, []);

    const getCategories = async () =>
    {
        try
        {
            const token = await AsyncStorage.getItem('token');
            setToken(token)
            const response = await axios.get('http://192.168.0.117:5295/api/Categories', {
                headers: {
                    Authorization: `Bearer ${ token }`
                }
            });
            setCategories(response.data);
        } catch (error)
        {
            console.error('Error fetching categories: ', error);
        }
    };

    const handleSubmit = async () =>
    {
        try
        {
            const frequencyIndex = frequencyIndexMap[frequency];
            console.log(name, startDate, endDate, selectedCategoryValue, amount, frequency._index)
            const response = await axios.post('http://192.168.0.117:5295/api/subscriptions', {
                name: name,
                amount: parseFloat(amount),
                frequency: frequencyIndex,
                startDate: startDate,
                endDate: endDate,
                categoryId: selectedCategoryValue
            }, {
                headers: {
                    Authorization: `Bearer ${ token }`
                }
            });
            Alert.alert('Success', 'Subscription created successfully');
        } catch (error)
        {
            console.error('Error creating subscription: ', error);
            Alert.alert('Error', 'Failed to create subscription');
        }
    };

    const handleStartDateChange = (event, selectedDate) =>
    {
        const currentDate = selectedDate || startDate;
        setShowStartDatePicker(false);
        setStartDate(currentDate);
    };

    const handleEndDateChange = (event, selectedDate) =>
    {
        const currentDate = selectedDate || endDate;
        setShowEndDatePicker(false);
        setEndDate(currentDate);
    };

    const showStartDatePickerModal = () =>
    {
        setShowStartDatePicker(true);
    };

    const showEndDatePickerModal = () =>
    {
        setShowEndDatePicker(true);
    };

    return (
        <View style={ styles.container }>
            <View style={ styles.inputContainer }>
                <Text style={ styles.label }>Subscription Name</Text>
                <TextInput
                    style={ styles.input }
                    placeholder="Subscription Name"
                    onChangeText={ text => setName(text) }
                    value={ name }
                />
            </View>

            <View style={ styles.inputContainer }>
                <Text style={ styles.label }>Amount</Text>
                <TextInput
                    style={ styles.input }
                    placeholder="Amount"
                    onChangeText={ text => setAmount(text) }
                    value={ amount }
                    keyboardType="numeric"
                />
            </View>

            <TouchableOpacity style={ styles.inputContainer } onPress={ showStartDatePickerModal }>
                <Text style={ styles.label }>Start Date</Text>
                <View style={ styles.datePickerContainer }>
                    <Text style={ styles.dateText }>{ startDate.toLocaleDateString('en-US', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    }) }</Text>
                    { showStartDatePicker && (
                        <DateTimePicker
                            value={ startDate }
                            mode="date"
                            display="spinner"
                            onChange={ handleStartDateChange }
                        />
                    ) }
                </View>
            </TouchableOpacity>

            <TouchableOpacity style={ styles.inputContainer } onPress={ showEndDatePickerModal }>
                <Text style={ styles.label }>End Date</Text>
                <View style={ styles.datePickerContainer }>
                    <Text style={ styles.dateText }>{ endDate.toLocaleDateString('en-US', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    }) }</Text>
                    { showEndDatePicker && (
                        <DateTimePicker
                            value={ endDate }
                            mode="date"
                            display="spinner"
                            onChange={ handleEndDateChange }
                        />
                    ) }
                </View>
            </TouchableOpacity>

            <View style={ styles.inputContainer }>
                <Text style={ styles.label }>Frequency</Text>
                <Dropdown
                    data={ Object.values(SubscriptionFrequency).map((option, index) => ({
                        value: option,
                        label: option,
                        index
                    })) }
                    value={ frequency }
                    onChange={ (item) => setFrequency(item.value) }
                    labelField="label"
                    valueField="value"
                    placeholder="Select a Frequency"
                    style={ styles.dropdown }
                />
            </View>


            <View style={ styles.inputContainer }>
                <Dropdown
                    data={ categories }
                    labelField="name"
                    valueField="categoryId"
                    mode='default'
                    style={ styles.dropdown }
                    placeholder='Select a Category'
                    value={ selectedCategoryValue }
                    onChange={ (item) => setSelectedCategoryValue(item?.categoryId) }
                />
            </View>

            <Button title="Create" onPress={ handleSubmit } />
        </View>
    );
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
