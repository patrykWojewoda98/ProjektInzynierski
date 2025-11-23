import { useRouter } from "expo-router";
import React, { useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Image,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const LoginScreen = () => {
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

      if (client.investProfile == null) {
        router.push("/create-invest-profile");
      } else {
        router.push("/main-menu");
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

  return (
    <ScrollView
      contentContainerStyle={[
        globalStyles.centerContainer,
        globalStyles.containerPadding,
      ]}
    >
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, globalStyles.logoLarge]}
      />

      <Text style={[globalStyles.header, spacing.mb5]}>Login</Text>

      <View style={[globalStyles.section, globalStyles.formContainer]}>
        <Text style={globalStyles.label}>Client ID</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          placeholder="Enter your client ID"
          placeholderTextColor={COLORS.placeholderGrey}
          value={clientId}
          onChangeText={setClientId}
          keyboardType="number-pad"
          autoCapitalize="none"
        />

        <TouchableOpacity
          style={[
            globalStyles.button,
            globalStyles.fullWidth,
            isLoading && globalStyles.buttonDisabled,
            spacing.py3,
          ]}
          onPress={handleLogin}
          disabled={isLoading}
        >
          {isLoading ? (
            <ActivityIndicator color={COLORS.whiteHeader} />
          ) : (
            <Text style={globalStyles.buttonText}>Log in</Text>
          )}
        </TouchableOpacity>

        <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
          <Text style={[globalStyles.text, spacing.mr1]}>
            Don't have an account?{" "}
          </Text>
          <TouchableOpacity onPress={() => router.push("/auth/register")}>
            <Text style={globalStyles.link}>Register</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

export default LoginScreen;
