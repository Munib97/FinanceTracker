import axios from "axios";
import React, { useEffect, useState } from "react";
import { View } from "react-native";
import { FlatList, ScrollView } from "react-native-gesture-handler";
import { ActivityIndicator } from "react-native";
import homeStyles from "../styles/homeStyles";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { parseISO, isToday, addWeeks, addMonths, addYears, isWithinInterval, isYesterday, format } from "date-fns";
import { Text } from 'react-native-elements'

export default function Home()
{
    const [combinedData, setCombinedData] = useState([])
    const [loading, setLoading] = useState(true)
    const [currentMonthSpending, setCurrentMonthSpending] = useState(0);



    useEffect(() =>
    {
        fetchData()
        fetchCurrentMonthSpendings()
    }, [])


    const fetchData = async () =>
    {
        setLoading(true)
        try
        {
            const token = await AsyncStorage.getItem('token')
            const response = await axios.get(`http://192.168.0.117:5295/api/CombinedSpendings`, {
                headers: {
                    Authorization: `Bearer ${ token }`,
                }
            })
            setCombinedData(response.data)

        } catch (error)
        {
            console.error("Error fetching data: ", error)
        } finally
        {
            setLoading(false)
        }
    }



    const fetchCurrentMonthSpendings = async () =>
    {
        setCurrentMonthSpending(null)
        try
        {
            const token = await AsyncStorage.getItem('token')


            const response = await axios.get(`http://192.168.0.117:5295/api/CombinedSpendings/currentMonthSpending/user/`, {
                headers: {
                    Authorization: `Bearer ${ token }`,
                }
            })
            setCurrentMonthSpending(response.data)


        } catch (error)
        {
            console.error("Error fetching current month spendings: ", error)
        }
    }
    const renderHeader = (date) =>
    {
        if (isToday(date)) return "Today"
        if (isYesterday(date)) return "Yesterday"
        return format(date, "EEEE, MMMM do")
    }
    const renderItem = ({ item }) =>
    {
        return (
            <View style={ homeStyles.expenseItem }>
                <Text style={ homeStyles.expenseDescription }>{ item.name }</Text>
                <Text style={ homeStyles.expenseAmount }>€{ item.amount }</Text>
            </View>
        )
    }

    return (
        <ScrollView>
            <View style={ homeStyles.container }>
                <Text style={ homeStyles.header }>Finance Tracker</Text>
                { loading ? (
                    <ActivityIndicator size="large" color="#007BFF" />
                ) : (
                    <View>
                        <Text style={ homeStyles.totalSpent }>
                            { `Total Money Spent This Month: ${ currentMonthSpending }` }
                        </Text>
                        <FlatList
                            style={ homeStyles.list }
                            data={ combinedData }
                            keyExtractor={ (item, index) => index.toString() }
                            renderItem={ ({ item }) => (
                                <View>
                                    <Text h4>{ renderHeader(parseISO(item.date)) }</Text>
                                    { renderItem({ item }) }
                                </View>
                            ) }
                        />
                    </View>

                ) }
            </View>
        </ScrollView>
    );
}