import AsyncStorage from "@react-native-async-storage/async-storage";
import { useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
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
import { decodeToken } from "../../utils/decodeToken";

const LoginScreen = () => {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isCheckingToken, setIsCheckingToken] = useState(true);

  // Sprawdzenie tokenu po uruchomieniu widoku
  useEffect(() => {
    const checkToken = async () => {
      try {
        const token = await AsyncStorage.getItem("userToken");

        if (!token) {
          setIsCheckingToken(false);
          return;
        }

        const decoded = decodeToken(token);
        const now = Date.now() / 1000;

        if (!decoded || (decoded.exp && decoded.exp < now)) {
          console.log("❌ Token expired or invalid — removing it...");
          await AsyncStorage.removeItem("userToken");
          setIsCheckingToken(false);
          return;
        }

        console.log("✅ Token valid — skipping login");
        router.replace(ROUTES.MAIN_MENU);
      } catch (error) {
        console.error("Token check error:", error);
        await AsyncStorage.removeItem("userToken");
        setIsCheckingToken(false);
      }
    };

    checkToken();
  }, []);

  const handleLogin = async () => {
    if (!email.trim() || !password.trim()) {
      Alert.alert("Error", "Please enter your email and password");
      return;
    }

    setIsLoading(true);

    try {
      const response = await ApiService.login({
        email: email,
        password: password,
      });

      if (response?.message) {
        Alert.alert("Info", response.message);
      }

      if (response?.token) {
        router.replace(ROUTES.MAIN_MENU);
        return;
      }
      setTimeout(() => {
        router.reload();
      }, 200);

      Alert.alert("Error", "Invalid email or/and password");
    } catch (error) {
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
        {/* EMAIL */}
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

        {/* PASSWORD */}
        <Text style={globalStyles.label}>Password</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          placeholder="Enter your password"
          placeholderTextColor={COLORS.placeholderGrey}
          secureTextEntry
          value={password}
          onChangeText={setPassword}
        />

        {/* LOGIN BUTTON */}
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

        {/* REGISTER LINK */}
        <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
          <Text style={[globalStyles.text, spacing.mr1]}>
            Don't have an account?{" "}
          </Text>
          <TouchableOpacity onPress={() => router.push("/auth/register")}>
            <Text style={globalStyles.link}>Register</Text>
          </TouchableOpacity>
        </View>
      </View>

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default LoginScreen;
