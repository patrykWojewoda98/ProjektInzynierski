import { useState, useEffect } from "react";
import {
  View,
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
} from "react-native";
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { ROUTES } from "../../routes";

const AddRegionCodeScreen = () => {
  const router = useRouter();
  const [code, setCode] = useState("");
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  const handleSave = async () => {
    if (!code.trim()) {
      Alert.alert("Validation error", "Code is required.");
      return;
    }

    setSaving(true);
    try {
      await ApiService.createRegionCode({ code });
      Alert.alert("Success", "Region code created.");
      router.replace(ROUTES.REGION_CODE);
    } catch {
      Alert.alert("Error", "Failed to create region code.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Region Code</Text>

      <Text style={globalStyles.label}>Code</Text>
      <TextInput
        style={[globalStyles.input, spacing.mb4]}
        value={code}
        onChangeText={setCode}
      />

      <TouchableOpacity
        style={[globalStyles.button, saving && globalStyles.buttonDisabled]}
        disabled={saving}
        onPress={handleSave}
      >
        <Text style={globalStyles.buttonText}>Save</Text>
      </TouchableOpacity>

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default AddRegionCodeScreen;
