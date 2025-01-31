import axios from "axios";
import React, { useEffect, useState } from "react";
import { View, StyleSheet, Dimensions, Button, Platform } from "react-native";
import { FlatList, ScrollView } from "react-native-gesture-handler";
import { ActivityIndicator } from "react-native";
import { Text } from 'react-native-elements';
import { PieChart } from 'react-native-chart-kit';
import AsyncStorage from "@react-native-async-storage/async-storage";
import DateTimePicker from "@react-native-community/datetimepicker";
import { format, startOfMonth, endOfMonth, isToday, isYesterday, differenceInDays, subWeeks, subMonths, subYears } from "date-fns";

const screenWidth = Dimensions.get('window').width;
const predefinedColors = [
    "#FF6347", "#FFA07A", "#FFD700", "#32CD32", "#1E90FF", "#8A2BE2",
    "#FF4500", "#D2691E", "#FF1493", "#00BFFF"
];

export default function Home()
{
    const [combinedData, setCombinedData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [currentMonthSpending, setCurrentMonthSpending] = useState(0);
    const [spendingByCategory, setSpendingByCategory] = useState([]);
    const [filteredData, setFilteredData] = useState([]);
    const [filter, setFilter] = useState('all');
    const [startDate, setStartDate] = useState(new Date(startOfMonth(new Date())));
    const [endDate, setEndDate] = useState(new Date(endOfMonth(new Date())));
    const [showStartPicker, setShowStartPicker] = useState(false);
    const [showEndPicker, setShowEndPicker] = useState(false);
    const [showFilters, setShowFilters] = useState(false);
    const [categorizedData, setCategorizedData] = useState({});

    useEffect(() =>
    {
        fetchData(startDate, endDate);
    }, [startDate, endDate]);

    useEffect(() =>
    {
        applyFilter();
    }, [filter, combinedData]);

    useEffect(() =>
    {
        categorizeItems(filteredData);
    }, [filteredData]);

    const fetchData = async (start, end) =>
    {
        setLoading(true);
        try
        {
            const token = await AsyncStorage.getItem('token');
            const formattedStart = format(start, 'yyyy-MM-dd');
            const formattedEnd = format(end, 'yyyy-MM-dd');
            console.log(`Fetching data from ${ formattedStart } to ${ formattedEnd }`);
            const response = await axios.get(`http://192.168.0.2:5295/api/CombinedSpendings`, {
                params: { startDate: formattedStart, endDate: formattedEnd },
                headers: { Authorization: `Bearer ${ token }` },
            });
            const data = response.data;
            console.log("Fetched data: ", data);
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

    const applyFilter = () =>
    {
        let filtered = combinedData.filter(item =>
        {
            const itemDate = new Date(item.date);
            return itemDate >= startDate && itemDate <= endDate;
        });

        if (filter !== 'all')
        {
            filtered = filtered.filter(item => item.categoryName === filter);
        }

        setFilteredData(filtered);
    };

    const calculateSpendingByCategory = (data) =>
    {
        const categoryMap = {};
        let totalSpending = 0;

        data.forEach(item =>
        {
            const category = item.categoryName || 'Uncategorized';
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
            percentage: ((categoryMap[category] / totalSpending) * 100).toFixed(2),
        }));

        setSpendingByCategory(spendingByCategory);
        setCurrentMonthSpending(totalSpending);
    };

    const categorizeItems = (items) =>
    {
        const groupedData = {
            today: [],
            yesterday: [],
            lastWeek: [],
            lastMonth: [],
            lastYear: [],
            older: [],
        };

        items.forEach(item =>
        {
            const itemDate = new Date(item.date);

            if (isToday(itemDate))
            {
                groupedData.today.push(item);
            } else if (isYesterday(itemDate))
            {
                groupedData.yesterday.push(item);
            } else if (differenceInDays(new Date(), itemDate) <= 7)
            {
                groupedData.lastWeek.push(item);
            } else if (differenceInDays(new Date(), itemDate) <= 30)
            {
                groupedData.lastMonth.push(item);
            } else if (differenceInDays(new Date(), itemDate) <= 365)
            {
                groupedData.lastYear.push(item);
            } else
            {
                groupedData.older.push(item);
            }
        });

        setCategorizedData(groupedData);
    };

    const pieChartData = spendingByCategory.map((item, index) => ({
        name: item.category,
        amount: item.amount,
        color: predefinedColors[index % predefinedColors.length],
        legendFontColor: '#333333',
        legendFontSize: 15,
    }));

    const renderCardItem = ({ item }) =>
    {
        const categoryColor = pieChartData.find(pieItem => pieItem.name === item.categoryName)?.color || '#FF6347';
        const formattedDate = format(new Date(item.date), 'MMM d, yyyy h:mm a');

        return (
            <View style={ styles.card }>
                <View style={ [styles.categoryStrip, { backgroundColor: categoryColor }] } />
                <Text style={ styles.cardHeading }>{ item.name }</Text>
                <Text style={ styles.cardAmount }>€{ item.amount }</Text>
                <Text style={ styles.cardDate }>{ formattedDate }</Text>
            </View>
        );
    };

    const renderItemsWithHeadings = (categorizedData) =>
    {
        const headings = [
            { title: 'Today', data: categorizedData.today },
            { title: 'Yesterday', data: categorizedData.yesterday },
            { title: 'Last Week', data: categorizedData.lastWeek },
            { title: 'Last Month', data: categorizedData.lastMonth },
            { title: 'Last Year', data: categorizedData.lastYear },
            { title: 'Older', data: categorizedData.older },
        ];

        return headings.map((heading, index) =>
        {
            if (heading.data.length === 0) return null;

            return (
                <View key={ index } style={ styles.section }>
                    <Text style={ styles.heading }>{ heading.title }</Text>
                    <FlatList
                        data={ heading.data }
                        renderItem={ renderCardItem }
                        keyExtractor={ (item, index) => index.toString() }
                    />
                </View>
            );
        });
    };

    return (
        <ScrollView style={ styles.container }>
            <View style={ styles.headerContainer }>
                <Text style={ styles.totalSpent }>Total spent: €{ currentMonthSpending }</Text>
                <View style={ styles.chartWrapper }>
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
                            style: { borderRadius: 16 },
                        } }
                        accessor="amount"
                        backgroundColor="transparent"
                        paddingLeft="15"
                        absolute
                        hasLegend={ false }
                        center={ [150, 0] }
                    />
                </View>
            </View>

            <View style={ styles.chartWrapper }>
                <View style={ styles.categoryItemsContainer }>
                    { spendingByCategory.map((item, index) => (
                        <View key={ index } style={ styles.categoryItem }>
                            <View style={ [styles.colorBox, { backgroundColor: pieChartData[index].color }] } />
                            <Text style={ styles.categoryText }>{ item.category }: €{ item.amount }</Text>
                        </View>
                    )) }
                </View>
            </View>

            <View style={ styles.filtersContainer }>
                <Button
                    title={ showFilters ? "Hide Filters" : "Show Filters" }
                    onPress={ () => setShowFilters(!showFilters) }
                />
                { showFilters && (
                    <View>
                        <Button title="Select Start Date" onPress={ () => setShowStartPicker(true) } />
                        { showStartPicker && (
                            <DateTimePicker
                                value={ startDate }
                                mode="date"
                                onChange={ (event, selectedDate) =>
                                {
                                    setShowStartPicker(false);
                                    if (selectedDate) setStartDate(selectedDate);
                                } }
                            />
                        ) }
                        <Button title="Select End Date" onPress={ () => setShowEndPicker(true) } />
                        { showEndPicker && (
                            <DateTimePicker
                                value={ endDate }
                                mode="date"
                                onChange={ (event, selectedDate) =>
                                {
                                    setShowEndPicker(false);
                                    if (selectedDate) setEndDate(selectedDate);
                                } }
                            />
                        ) }
                    </View>
                ) }
            </View>

            { loading ? (
                <ActivityIndicator size="large" color="#1E90FF" />
            ) : (
                renderItemsWithHeadings(categorizedData)
            ) }
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#ffffff',
    },
    headerContainer: {
        paddingTop: 110,
        backgroundColor: '#1E90FF',
        alignItems: 'center',
        height: 150,
        justifyContent: 'center',
    },
    totalSpent: {
        fontSize: 20,
        fontWeight: 'bold',
        color: 'white',
        paddingTop: 10,
    },
    chartWrapper: {
        paddingHorizontal: 20,
    },
    categoryItemsContainer: {
        marginTop: 20,
    },
    categoryItem: {
        flexDirection: 'row',
        alignItems: 'center',
        marginVertical: 5,
    },
    colorBox: {
        width: 15,
        height: 15,
        borderRadius: 8,
        marginRight: 10,
    },
    categoryText: {
        fontSize: 16,
    },
    filtersContainer: {
        marginTop: 20,
        paddingHorizontal: 20,
    },
    datePickerContainer: {
        marginVertical: 10,
    },
    section: {
        marginBottom: 20,
    },
    heading: {
        fontSize: 18,
        fontWeight: 'bold',
        marginBottom: 10,
    },
    card: {
        width: '100%',
        padding: 20,
        backgroundColor: '#f9f9f9',
        borderRadius: 8,
        marginBottom: 10,
        borderWidth: 1,
        borderColor: '#ddd',
        position: 'relative',
    },
    cardHeading: {
        fontSize: 16,
        fontWeight: 'bold',
        color: '#333',
    },
    cardAmount: {
        fontSize: 20,
        color: '#FF6347',
        textAlign: 'right',
    },
    categoryStrip: {
        width: '100%',
        height: 8,
        backgroundColor: '#1E90FF',
        borderRadius: 5,
        marginBottom: 10,
    },
    cardDate: {
        fontSize: 12,
        color: '#888',
        marginTop: 5,
    },
});
