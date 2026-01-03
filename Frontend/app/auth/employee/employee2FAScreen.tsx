import { useLocalSearchParams, useRouter } from "expo-router";
import React, { useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import ApiService from "../../../services/api";
import { globalStyles, spacing } from "../../../assets/styles/styles";
import { ROUTES } from "../../../routes";

const EmployeeTwoFactorScreen = () => {
  const router = useRouter();
  const { employeeId } = useLocalSearchParams();

  const [code, setCode] = useState("");
  const [loading, setLoading] = useState(false);

  const handleVerify = async () => {
    if (code.length !== 6) {
      Alert.alert("Error", "Enter 6-digit code");
      return;
    }

    setLoading(true);

    try {
      const response = await ApiService.verifyEmployee2FA({
        employeeId: Number(employeeId),
        code,
      });

      if (response?.message) {
        Alert.alert(response.message);
      }

      // ✅ token już zapisany → można wejść
      router.replace(ROUTES.EMPLOYEE_MAIN_MENU);
    } catch (e: any) {
      Alert.alert("Error", e.message || "Invalid verification code");
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.centerContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Two-Factor Verification
      </Text>

      <Text style={[globalStyles.text, spacing.mb3]}>
        Enter the 6-digit code sent to your email
      </Text>

      <TextInput
        style={[globalStyles.input, spacing.mb4]}
        keyboardType="number-pad"
        maxLength={6}
        value={code}
        onChangeText={setCode}
      />

      <TouchableOpacity
        style={globalStyles.button}
        onPress={handleVerify}
        disabled={loading}
      >
        {loading ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Verify</Text>
        )}
      </TouchableOpacity>
    </ScrollView>
  );
};

export default EmployeeTwoFactorScreen;
