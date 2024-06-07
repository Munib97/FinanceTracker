import { StyleSheet } from "react-native";
export const loginStyles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#F5F5F5',
        justifyContent: 'center',
        alignItems: 'center',
    },
    header: {
        flex: 1,
        justifyContent: 'center',
    },
    headerContent: {
        flexDirection: 'column',
        alignItems: 'center',
        marginTop: 80,
    },
    headerText: {
        fontSize: 32,
        fontWeight: 'bold',
        color: '#333',
    },
    userIcon: {
        paddingTop: 40,
    },
    form: {
        flex: 2,
        width: '80%',
        justifyContent: 'center',
    },
    input: {
        backgroundColor: '#FFF',
        borderRadius: 8,
        padding: 12,
        marginBottom: 16,
    },
    loginButton: {
        backgroundColor: '#007BFF',
        padding: 16,
        borderRadius: 8,
        alignItems: 'center',
    },
    loginButtonText: {
        color: '#FFF',
        fontSize: 16,
        fontWeight: 'bold',
    },
});
