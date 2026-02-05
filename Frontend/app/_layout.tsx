import AsyncStorage from "@react-native-async-storage/async-storage";
import { Slot, useRouter } from "expo-router";
import React, { createContext, useEffect, useState } from "react";
import { Image, Pressable, Text, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { globalStyles } from "../assets/styles/styles";
import { decodeToken } from "../utils/decodeToken";

export const AuthContext = createContext<any>({
  user: null,
  userType: null,
  token: "",
  logout: () => {},
  refreshAuth: () => {},
});

export default function RootLayout() {
  const [user, setUser] = useState<any>(null);
  const [token, setToken] = useState<string>("");
  const [userType, setUserType] = useState<"client" | "employee" | null>(null);

  const router = useRouter();

  const refreshAuth = async () => {
    const clientToken = await AsyncStorage.getItem("userToken");
    const employeeToken = await AsyncStorage.getItem("employeeToken");

    if (employeeToken) {
      const decoded = decodeToken(employeeToken);
      if (decoded && (!decoded.exp || decoded.exp > Date.now() / 1000)) {
        setToken(employeeToken);
        setUserType("employee");
        setUser({
          id: decoded.id,
          name: decoded.name,
          isAdmin: decoded.isAdmin,
        });
        return;
      } else {
        await AsyncStorage.removeItem("employeeToken");
      }
    }

    if (clientToken) {
      const decoded = decodeToken(clientToken);
      if (decoded && (!decoded.exp || decoded.exp > Date.now() / 1000)) {
        setToken(clientToken);
        setUserType("client");
        setUser({
          id: decoded.id,
          name: decoded.name,
        });
        return;
      } else {
        await AsyncStorage.removeItem("userToken");
      }
    }

    setUser(null);
    setToken("");
    setUserType(null);
  };

  useEffect(() => {
    refreshAuth();
  }, []);

  const logout = async () => {
    await AsyncStorage.multiRemove(["userToken", "employeeToken"]);
    await refreshAuth();
    router.replace("/");
  };

  const Header = () => (
    <View style={globalStyles.topHeader}>
      <View style={[globalStyles.row, globalStyles.alignCenter]}>
        <Image
          source={require("../assets/images/Logo.png")}
          style={globalStyles.logoSmall}
        />
        {user && (
          <Text style={[globalStyles.text, { color: "white", marginLeft: 10 }]}>
            Welcome, {user.name}
          </Text>
        )}
      </View>
      {token && (
        <Pressable onPress={logout} style={globalStyles.buttonSmall}>
          <Text style={globalStyles.buttonText}>Logout</Text>
        </Pressable>
      )}
    </View>
  );

  return (
    <AuthContext.Provider
      value={{ user, userType, token, logout, refreshAuth }}
    >
      <SafeAreaView style={[globalStyles.container, globalStyles.flex1]}>
        <Header />
        <View style={[globalStyles.flex1, globalStyles.containerPadding]}>
          <Slot />
        </View>
      </SafeAreaView>
    </AuthContext.Provider>
  );
}
