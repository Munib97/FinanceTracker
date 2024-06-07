import { StyleSheet } from "react-native";

export default homeStyles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#F5F5F5',
        padding: 0,
        marginTop: 50,
    },
    header: {
        fontSize: 32,
        fontWeight: 'bold',
        color: '#333',
        marginBottom: 20,
    },
    totalSpent: {
        fontSize: 18,
        fontWeight: 'bold',
        marginBottom: 20,
    },
    expenseItem: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginBottom: 10,
        paddingTop: 8,
        backgroundColor: 'white',
        height: 40,
    },
    expenseDescription: {
        fontSize: 16,
        color: '#333',
    },
    expenseAmount: {
        fontSize: 16,
        color: '#007BFF',
        fontWeight: 'bold',
    },
    list: {
        padding: 0,
        margin: 0,
    }
});