import { Slot, useRouter } from "expo-router";
import React, { createContext, useState } from "react";
import { Image, Pressable, Text, View } from "react-native";
import { globalStyles } from "../assets/styles/styles";

// Auth context to hold user/token/clientId globally
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
    // Clear auth-related global state
    setToken("");
    setClientId("");
    setUser(null);
    // Navigate to login
    router.replace("/auth/login");
  };

  const Header = () => {
    return (
      <View style={globalStyles.headerContainer}>
        <View style={globalStyles.headerLeft}>
          <Image
            source={require("../assets/images/Logo.png")}
            style={globalStyles.headerLogo}
          />
          {user && user.name ? (
            <Text style={globalStyles.headerWelcome}>Welcome, {user.name}</Text>
          ) : null}
        </View>

        <Pressable
          onPress={handleLogout}
          style={globalStyles.headerLogoutButton}
        >
          <Text style={globalStyles.headerLogoutText}>Logout</Text>
        </Pressable>
      </View>
    );
  };

  return (
    <AuthContext.Provider
      value={{ user, setUser, token, setToken, clientId, setClientId }}
    >
      <View style={globalStyles.rootContainer}>
        <Header />
        <View style={globalStyles.contentWrapper}>
          <Slot />
        </View>
      </View>
    </AuthContext.Provider>
  );
}
