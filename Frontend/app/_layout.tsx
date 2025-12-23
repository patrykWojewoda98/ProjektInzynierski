import AsyncStorage from "@react-native-async-storage/async-storage";
import { Slot, useRouter, useSegments } from "expo-router";
import React, { createContext, useEffect, useState } from "react";
import { Image, Pressable, Text, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { globalStyles } from "../assets/styles/styles";
import { decodeToken } from "../utils/decodeToken";

export const AuthContext = createContext<any>({
  user: null,
  setUser: (_u: any) => {},
  token: "",
  setToken: (_t: any) => {},
  clientId: "",
  setClientId: (_c: any) => {},
});

export default function RootLayout() {
  const [user, setUser] = useState<any>(null);
  const [token, setToken] = useState<string>("");
  const [clientId, setClientId] = useState<string>("");

  const router = useRouter();
  const segments = useSegments();

  // 🔹 Ładowanie użytkownika z tokenu (działa przy każdej zmianie ekranu)
  useEffect(() => {
    const loadUserFromToken = async () => {
      const storedToken = await AsyncStorage.getItem("userToken");

      if (storedToken) {
        setToken(storedToken);
        const decoded = decodeToken(storedToken);

        if (decoded) {
          setUser({
            name: decoded.name,
            id: decoded.id,
          });
        }
      } else {
        setToken("");
        setUser(null);
      }
    };

    loadUserFromToken();
  }, [segments]);

  const handleLogout = async () => {
    await AsyncStorage.removeItem("userToken");
    setToken("");
    setUser(null);
    router.replace("/auth/login");
  };

  const Header = () => (
    <View style={globalStyles.topHeader}>
      <View style={[globalStyles.row, globalStyles.alignCenter]}>
        <Image
          source={require("../assets/images/Logo.png")}
          style={globalStyles.logoSmall}
        />
        {user?.name ? (
          <Text
            style={[
              globalStyles.text,
              { color: "white", marginLeft: 10, fontWeight: "600" },
            ]}
          >
            Welcome, {user.name}
          </Text>
        ) : null}
      </View>

      {/* 🔹 Pokaż przycisk Logout tylko, jeśli istnieje token */}
      {token ? (
        <Pressable
          onPress={handleLogout}
          style={({ pressed }) => [
            globalStyles.buttonSmall,
            pressed && globalStyles.buttonPressed,
          ]}
        >
          <Text style={globalStyles.buttonText}>Logout</Text>
        </Pressable>
      ) : null}
    </View>
  );

  return (
    <AuthContext.Provider
      value={{ user, setUser, token, setToken, clientId, setClientId }}
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
