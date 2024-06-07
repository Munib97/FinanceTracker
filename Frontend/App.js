import React, { useState, useEffect, useContext } from 'react';
import { AuthContext, AuthProvider } from './authContext';
import AppNav from './AppNav';

export default function App()
{
  // const [authenticated, setAuthenticated] = useState(false);

  // useEffect(() =>
  // {
  //   async () =>
  //   {
  //     const token = await getTokenFromStorage();
  //     console.log(token)
  //     if (token)
  //     {
  //       setAuthenticated(true);
  //     } else
  //     {
  //       setAuthenticated(false);
  //     }
  //   }
  // }, []);


  // const getTokenFromStorage = async () =>
  // {
  //   const token = await AsyncStorage.getItem('token');
  //   return token
  // }
  // console.log(authenticated)
  return (
    <AuthProvider>
      <AppNav></AppNav>
    </AuthProvider>
  );
}