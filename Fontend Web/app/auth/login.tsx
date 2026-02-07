import { useRouter } from "expo-router";
import React, { useContext, useState } from "react";
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
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { AuthContext } from "../_layout";

const LoginScreen = () => {
  const router = useRouter();
  const { refreshAuth } = useContext(AuthContext);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async () => {
    if (!email.trim() || !password.trim()) {
      Alert.alert("Error", "Please enter your email and password");
      return;
    }

    setIsLoading(true);

    try {
      const response = await ApiService.login({
        email,
        password,
      });

      if (response?.token) {
        await refreshAuth(); // 🔥 TO ODŚWIEŻA LAYOUT
        router.replace(ROUTES.MAIN_MENU);
        return;
      }

      Alert.alert("Error", "Invalid email or/and password");
    } catch {
      Alert.alert("Error", "Invalid email or/and password");
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

      <Text style={[globalStyles.header, spacing.mb2]}>Login</Text>

      <View style={[globalStyles.section, globalStyles.formContainer]}>
        <Text style={globalStyles.label}>Email</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          placeholder="Enter your email"
          placeholderTextColor={COLORS.placeholderGrey}
          value={email}
          onChangeText={setEmail}
          autoCapitalize="none"
          keyboardType="email-address"
        />

        <Text style={globalStyles.label}>Password</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          placeholder="Enter your password"
          placeholderTextColor={COLORS.placeholderGrey}
          secureTextEntry
          value={password}
          onChangeText={setPassword}
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
            Don't have an account?
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
