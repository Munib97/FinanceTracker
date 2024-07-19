import axios from "axios";
import React, { useEffect, useState } from "react";
import { View, StyleSheet, Dimensions } from "react-native";
import { FlatList, ScrollView } from "react-native-gesture-handler";
import { ActivityIndicator } from "react-native";
import { Text } from 'react-native-elements';
import { PieChart } from 'react-native-chart-kit';
import RNPickerSelect from 'react-native-picker-select'; // Import Picker
import AsyncStorage from "@react-native-async-storage/async-storage";
import { parseISO, isToday, isYesterday, format, startOfWeek, endOfWeek, startOfMonth, endOfMonth, startOfDay } from "date-fns";

const screenWidth = Dimensions.get('window').width;

export default function Home()
{
    const [combinedData, setCombinedData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [currentMonthSpending, setCurrentMonthSpending] = useState(0);
    const [spendingByCategory, setSpendingByCategory] = useState([]);
    const [dateRange, setDateRange] = useState('thisMonth');

    useEffect(() =>
    {
        fetchData(dateRange);
        fetchCurrentMonthSpendings();
    }, [dateRange]);

    const fetchData = async (range) =>
    {
        setLoading(true);
        try
        {
            const token = await AsyncStorage.getItem('token');
            const startDate = getStartDate(range);
            const endDate = getEndDate(range);
            console.log(`Fetching data from ${ startDate } to ${ endDate }`); // Debugging line
            const response = await axios.get(`http://192.168.0.117:5295/api/CombinedSpendings`, {
                params: {
                    startDate,
                    endDate
                },
                headers: {
                    Authorization: `Bearer ${ token }`,
                }
            });
            const data = response.data;
            console.log("Fetched data: ", data); // Debugging line
            setCombinedData(data);
            calculateSpendingByCategory(data);
        } catch (error)
        {
            console.error("Error fetching data: ", error);
        } finally
        {
            setLoading(false);
        }
    };

    const fetchCurrentMonthSpendings = async () =>
    {
        setCurrentMonthSpending(null);
        try
        {
            const token = await AsyncStorage.getItem('token');
            const response = await axios.get(`http://192.168.0.117:5295/api/CombinedSpendings/currentMonthSpending/user/`, {
                headers: {
                    Authorization: `Bearer ${ token }`,
                }
            });
            setCurrentMonthSpending(response.data);
        } catch (error)
        {
            console.error("Error fetching current month spendings: ", error);
        }
    };

    const calculateSpendingByCategory = (data) =>
    {
        const categoryMap = {};
        let totalSpending = 0;

        data.forEach(item =>
        {
            const category = item.categoryName || 'Uncategorized'; // Handle undefined category
            if (categoryMap[category])
            {
                categoryMap[category] += item.amount;
            } else
            {
                categoryMap[category] = item.amount;
            }
            totalSpending += item.amount;
        });

        const spendingByCategory = Object.keys(categoryMap).map(category => ({
            category,
            amount: categoryMap[category],
            percentage: ((categoryMap[category] / totalSpending) * 100).toFixed(2) // Calculate percentage
        }));

        setSpendingByCategory(spendingByCategory);
    };

    const getStartDate = (range) =>
    {
        const now = new Date();
        switch (range)
        {
            case 'today':
                return format(startOfDay(now), 'yyyy-MM-dd');
            case 'thisWeek':
                return format(startOfWeek(now), 'yyyy-MM-dd');
            case 'thisMonth':
                return format(startOfMonth(now), 'yyyy-MM-dd');
            default:
                return format(startOfMonth(now), 'yyyy-MM-dd');
        }
    };

    const getEndDate = (range) =>
    {
        const now = new Date();
        switch (range)
        {
            case 'today':
                return format(now, 'yyyy-MM-dd');
            case 'thisWeek':
                return format(endOfWeek(now), 'yyyy-MM-dd');
            case 'thisMonth':
                return format(endOfMonth(now), 'yyyy-MM-dd');
            default:
                return format(endOfMonth(now), 'yyyy-MM-dd');
        }
    };

    const renderHeader = (date) =>
    {
        if (isToday(date)) return "Today";
        if (isYesterday(date)) return "Yesterday";
        return format(date, "EEEE, MMMM do");
    };

    const renderItem = ({ item }) => (
        <View style={ homeStyles.expenseItem }>
            <Text style={ homeStyles.expenseDescription }>{ item.name }</Text>
            <Text style={ homeStyles.expenseAmount }>€{ item.amount }</Text>
        </View>
    );

    const renderCategoryItem = ({ item }) => (
        <View style={ styles.categoryItem }>
            <Text style={ styles.categoryName }>{ item.category }</Text>
            <Text style={ styles.categoryAmount }>€{ item.amount } ({ item.percentage }%)</Text>
        </View>
    );

    const pieChartData = spendingByCategory.map(item => ({
        name: item.category,
        amount: item.amount,
        color: `#${ Math.floor(Math.random() * 16777215).toString(16) }`,
        legendFontColor: '#333333',
        legendFontSize: 15,
    }));

    return (
        <ScrollView style={ styles.container }>
            <View style={ styles.headerContainer }>
                <Text style={ styles.headerTitle }>Finance Tracker</Text>
                <RNPickerSelect
                    onValueChange={ (value) => setDateRange(value) }
                    items={ [
                        { label: 'Today', value: 'today' },
                        { label: 'This Week', value: 'thisWeek' },
                        { label: 'This Month', value: 'thisMonth' },
                    ] }
                    style={ pickerSelectStyles }
                />
            </View>
            <View style={ styles.contentContainer }>
                { loading ? (
                    <ActivityIndicator size="large" color="#007BFF" style={ styles.loadingIndicator } />
                ) : (
                    <View>
                        <Text style={ styles.totalSpent }>
                            { `Total Money Spent This Month: €${ currentMonthSpending }` }
                        </Text>
                        <PieChart
                            data={ pieChartData }
                            width={ screenWidth - 40 }
                            height={ 220 }
                            chartConfig={ {
                                backgroundColor: '#ffffff',
                                backgroundGradientFrom: '#ffffff',
                                backgroundGradientTo: '#ffffff',
                                decimalPlaces: 1,
                                color: (opacity = 1) => `rgba(26, 255, 146, ${ opacity })`,
                                style: {
                                    borderRadius: 16
                                }
                            } }
                            accessor="amount"
                            backgroundColor="transparent"
                            paddingLeft="15"
                            absolute
                        />
                        <FlatList
                            style={ styles.list }
                            data={ spendingByCategory }
                            keyExtractor={ (item, index) => index.toString() }
                            renderItem={ renderCategoryItem }
                            ListHeaderComponent={ <Text style={ styles.categoryHeader }>Spent On:</Text> }
                        />
                        <FlatList
                            style={ styles.list }
                            data={ combinedData }
                            keyExtractor={ (item, index) => index.toString() }
                            renderItem={ ({ item }) => (
                                <View style={ styles.listItemContainer }>
                                    <Text style={ styles.listHeader }>{ renderHeader(parseISO(item.date)) }</Text>
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

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#ffffff',
    },
    headerContainer: {
        padding: 20,
        backgroundColor: '#1E90FF',
        alignItems: 'center',
    },
    headerTitle: {
        fontSize: 24,
        fontWeight: 'bold',
        color: 'white',
    },
    contentContainer: {
        padding: 20,
    },
    loadingIndicator: {
        marginTop: 20,
    },
    totalSpent: {
        fontSize: 18,
        fontWeight: 'bold',
        marginBottom: 20,
        color: '#333333',
        textAlign: 'center',
    },
    list: {
        width: '100%',
    },
    listItemContainer: {
        marginBottom: 10,
    },
    listHeader: {
        fontSize: 20,
        fontWeight: 'bold',
        marginBottom: 10,
        color: '#1E90FF',
    },
    categoryHeader: {
        fontSize: 20,
        fontWeight: 'bold',
        marginBottom: 10,
        color: '#1E90FF',
        textAlign: 'center',
    },
    categoryItem: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        paddingVertical: 10,
        borderBottomWidth: 1,
        borderBottomColor: '#cccccc',
    },
    categoryName: {
        fontSize: 16,
        color: '#333333',
    },
    categoryAmount: {
        fontSize: 16,
        color: '#333333',
    },
});

const pickerSelectStyles = StyleSheet.create({
    inputIOS: {
        fontSize: 16,
        paddingVertical: 12,
        paddingHorizontal: 10,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 4,
        color: 'black',
        paddingRight: 30,
        marginBottom: 20,
    },
    inputAndroid: {
        fontSize: 16,
        paddingHorizontal: 10,
        paddingVertical: 8,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 8,
        color: 'black',
        marginBottom: 20,
    },
});
