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
import { globalStyles } from "../../assets/styles/styles";
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
        router.push("/(invest)/CreateInvestProfile");
      } else {
        router.push("/(tabs)");
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
    <ScrollView contentContainerStyle={globalStyles.centerContainer}>
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, { width: 250, height: 250 }]}
      />

      <Text style={[globalStyles.header, { marginBottom: 40 }]}>Login</Text>

      <View style={[globalStyles.section, { width: "90%", maxWidth: 400 }]}>
        <Text style={globalStyles.label}>Client ID</Text>
        <TextInput
          style={[globalStyles.input, { marginBottom: 30 }]}
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
            isLoading && globalStyles.buttonDisabled,
            { width: "100%" },
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

        <View
          style={{
            flexDirection: "row",
            justifyContent: "center",
            marginTop: 20,
          }}
        >
          <Text style={{ color: COLORS.textGrey }}>
            Don&apos;t have an account?{" "}
          </Text>
          <TouchableOpacity onPress={() => router.push("/auth/register")}>
            <Text style={{ color: COLORS.primary, fontWeight: "600" }}>
              Register
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

export default LoginScreen;
