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
import { globalStyles, spacing } from "../../../assets/styles/styles";
import { COLORS } from "../../../assets/Constants/colors";
import ApiService from "../../../services/api";
import { ROUTES } from "../../../routes";

const EmployeeLoginScreen = () => {
  const router = useRouter();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    if (!email || !password) {
      Alert.alert("Error", "Enter email and password");
      return;
    }

    setLoading(true);

    try {
      const res = await ApiService.loginEmployee({ email, password });

      router.push({
        pathname: ROUTES.EMPLOYEE_2FA,
        params: { employeeId: res.employeeId },
      });
    } catch (e) {
      Alert.alert("Error", e.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.centerContainer}>
      <Image
        source={require("../../../assets/images/Logo.png")}
        style={[globalStyles.logo, spacing.mb4]}
      />

      <Text style={[globalStyles.header, spacing.mb5]}>Employee Login</Text>

      <View style={globalStyles.formContainer}>
        <Text style={globalStyles.label}>Email</Text>
        <TextInput
          style={globalStyles.input}
          autoCapitalize="none"
          value={email}
          onChangeText={setEmail}
        />

        <Text style={globalStyles.label}>Password</Text>
        <TextInput
          style={globalStyles.input}
          secureTextEntry
          value={password}
          onChangeText={setPassword}
        />

        <TouchableOpacity
          style={[globalStyles.button, spacing.mt4]}
          onPress={handleLogin}
          disabled={loading}
        >
          {loading ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Log in</Text>
          )}
        </TouchableOpacity>
      </View>

      <TouchableOpacity onPress={() => router.back()}>
        <Text style={[globalStyles.link, spacing.mt5]}>Go back</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default EmployeeLoginScreen;
