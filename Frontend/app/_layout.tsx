import { Slot, useRouter } from "expo-router";
import React, { createContext, useState } from "react";
import { Image, Pressable, Text, View } from "react-native";
import { globalStyles } from "../assets/styles/styles";

// Auth context (globalny kontekst dla użytkownika, tokena i clientId)
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

  const handleLogout = () => {
    // Czyszczenie stanu logowania
    setToken("");
    setClientId("");
    setUser(null);
    router.replace("/auth/login");
  };

  const Header = () => {
    return (
      <View style={globalStyles.topHeader}>
        <View style={[globalStyles.row, globalStyles.alignCenter]}>
          <Image
            source={require("../assets/images/Logo.png")}
            style={globalStyles.logoSmall}
          />
          {user && user.name ? (
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

        <Pressable
          onPress={handleLogout}
          style={({ pressed }) => [
            globalStyles.buttonSmall,
            pressed && globalStyles.buttonPressed,
          ]}
        >
          <Text style={globalStyles.buttonText}>Logout</Text>
        </Pressable>
      </View>
    );
  };

  return (
    <AuthContext.Provider
      value={{ user, setUser, token, setToken, clientId, setClientId }}
    >
      <View style={[globalStyles.container, globalStyles.flex1]}>
        <Header />
        <View style={[globalStyles.flex1, globalStyles.containerPadding]}>
          <Slot />
        </View>
      </View>
    </AuthContext.Provider>
  );
}
