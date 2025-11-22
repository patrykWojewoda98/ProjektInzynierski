import { Stack, useRouter } from "expo-router";
import React from "react";
import { Image, Text, TouchableOpacity, View } from "react-native";
import { COLORS } from "../assets/Constants/colors";
import { globalStyles } from "../assets/styles/styles";

export default function HomeScreen() {
  const router = useRouter();

  const handleLogin = () => {
    router.push("/auth/login");
  };

  const handleRegister = () => {
    router.push("/auth/register");
  };

  return (
    <View style={globalStyles.centerContainer}>
      <Stack.Screen options={{ headerShown: false }} />

      <Text style={[globalStyles.header, { marginBottom: 20 }]}>Welcome</Text>

      <Image
        source={require("../assets/images/Logo.png")}
        style={[
          globalStyles.logo,
          { width: 250, height: 250, marginBottom: 40 },
        ]}
      />

      <View style={[globalStyles.section, { width: "80%" }]}>
        <TouchableOpacity
          style={[globalStyles.button, { padding: 20 }]}
          onPress={handleLogin}
        >
          <Text style={[globalStyles.buttonText, { fontSize: 18 }]}>
            Log In
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          style={[
            globalStyles.button,
            {
              backgroundColor: COLORS.mediumGrey,
              padding: 20,
              marginTop: 15,
            },
          ]}
          onPress={handleRegister}
        >
          <Text style={[globalStyles.buttonText, { fontSize: 18 }]}>
            Register
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}
