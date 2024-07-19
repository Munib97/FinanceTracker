import React, { useState, useEffect, useRef } from 'react';
import
{
    View,
    TouchableOpacity,
    StyleSheet,
    Text,
    Platform,
    Dimensions,
    ScrollView,
    Image,
} from 'react-native';
import { GestureHandlerRootView } from 'react-native-gesture-handler';
import ExpensesIcon from '../icons/ExpensesIcon.png';
import HomeIcon from '../icons/HomeIcon.png';
import SubscriptionsIcon from '../icons/SubscriptionsIcon.png';
import Expenses from './expenses';
import Home from './home';
import Subscriptions from './subscriptions';

const tabIcons = {
    E: ExpensesIcon,
    H: HomeIcon,
    S: SubscriptionsIcon,
};

const tabColors = {
    inactive: Platform.OS === 'android' ? 'lightgrey' : 'darkgrey',
    active: 'darkgrey',
};

const screenWidth = Dimensions.get('window').width;

export default function Dashboard()
{
    const [activeIndex, setActiveIndex] = useState(0);
    const scrollViewRef = useRef(null);

    const handleTabPress = (index) =>
    {
        setActiveIndex(index);
        scrollToIndex(index);
    };

    const scrollToIndex = (index) =>
    {
        if (scrollViewRef.current)
        {
            scrollViewRef.current.scrollTo({
                x: screenWidth * index,
                animated: true,
            });
        }
    };

    const onScroll = (event) =>
    {
        const contentOffsetX = event.nativeEvent.contentOffset.x;
        const index = Math.round(contentOffsetX / screenWidth);
        if (index !== activeIndex)
        {
            setActiveIndex(index);
        }
    };

    const data = [
        { tabLabel: 'E', component: Expenses },
        { tabLabel: 'H', component: Home },
        { tabLabel: 'S', component: Subscriptions },
    ];

    const renderTab = (tabData, index) => (
        <TouchableOpacity
            key={ index }
            style={ [styles.tabButton, index === activeIndex && styles.activeTabButton] }
            onPress={ () => handleTabPress(index) }>
            <Image
                source={ tabIcons[tabData.tabLabel] }
                style={ [styles.tabIcon, index === activeIndex && styles.activeIcon] }
                resizeMode="contain"
            />
        </TouchableOpacity>
    );

    return (
        <GestureHandlerRootView style={ styles.container }>
            <ScrollView
                ref={ scrollViewRef }
                horizontal
                pagingEnabled
                showsHorizontalScrollIndicator={ false }
                contentContainerStyle={ { width: screenWidth * data.length } }
                scrollEventThrottle={ 16 }
                onScroll={ onScroll }>
                { data.map((tab, index) => (
                    <View key={ index } style={ { width: screenWidth, paddingHorizontal: 10 } }>
                        <tab.component />
                    </View>
                )) }
            </ScrollView>
            <View style={ styles.bottomTabsContainer }>
                { data.map((tabData, index) => renderTab(tabData, index)) }
            </View>
        </GestureHandlerRootView>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
    },
    bottomTabsContainer: {
        flexDirection: 'row',
        justifyContent: 'space-around',
        backgroundColor: tabColors.inactive,
        height: 50,
    },
    tabButton: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    activeTabButton: {
        backgroundColor: tabColors.active,
    },
    tabIcon: {
        width: 30,
        height: 30,
    },
    activeIcon: {
        tintColor: 'red',
    },
});
