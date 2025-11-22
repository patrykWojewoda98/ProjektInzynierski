import { Stack, useRouter } from "expo-router";
import React, { useState } from "react";
import {
  Alert,
  Image,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../assets/Constants/colors";
import { globalStyles } from "../assets/styles/styles";
import ApiService from "../services/api";

export default function HomeScreen() {
  const router = useRouter();
  const [clientId, setClientId] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async () => {
    if (!clientId.trim()) {
      Alert.alert("Error", "Please enter your client ID");
      return;
    }

    setIsLoading(true);
    try {
      const client = await ApiService.getClientById(clientId);

      if (!client.investProfile) {
        router.push("/invest/createInvestProfile");
      } else {
        router.push("/mainMenu");
      }
    } catch (error) {
      Alert.alert(
        "Error",
        Array.isArray(error) ? error[0] : "An error occurred"
      );
    } finally {
      setIsLoading(false);
    }
  };

  const handleRegister = () => {
    router.push("/auth/register");
  };
  // Temporary navigation to main menu for testing
  const handleMoveToMainManu = () => {
    router.push("/mainMenu");
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
        <Text style={globalStyles.label}>Enter Client ID</Text>
        <TextInput
          style={globalStyles.input}
          placeholder="Client ID"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="number-pad"
          value={clientId}
          onChangeText={setClientId}
        />

        <TouchableOpacity
          style={[globalStyles.button, { padding: 20 }]}
          onPress={handleLogin}
        >
          <Text style={[globalStyles.buttonText, { fontSize: 18 }]}>
            {isLoading ? "Loading..." : "Log In"}
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

        <TouchableOpacity
          style={[
            globalStyles.button,
            {
              backgroundColor: COLORS.mediumGrey,
              padding: 20,
              marginTop: 15,
            },
          ]}
          onPress={handleMoveToMainManu}
        >
          <Text style={[globalStyles.buttonText, { fontSize: 18 }]}>
            Menu Głowne (test)
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}
