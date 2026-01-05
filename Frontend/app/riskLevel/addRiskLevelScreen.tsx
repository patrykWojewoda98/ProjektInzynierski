import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const AddRiskLevelScreen = () => {
  const router = useRouter();
  const [riskScale, setRiskScale] = useState("");
  const [description, setDescription] = useState("");
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  const handleSave = async () => {
    if (!riskScale || !description.trim()) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    try {
      await ApiService.createRiskLevel({
        riskScale: Number(riskScale),
        description,
      });
      Alert.alert("Success", "Risk level created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create risk level.");
    }
  };

  if (!isReady) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Risk Level</Text>

      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Risk Scale</Text>
        <TextInput
          style={globalStyles.input}
          keyboardType="numeric"
          value={riskScale}
          onChangeText={setRiskScale}
        />

        <Text style={globalStyles.label}>Description</Text>
        <TextInput
          style={globalStyles.input}
          value={description}
          onChangeText={setDescription}
        />
      </View>

      <TouchableOpacity style={globalStyles.button} onPress={handleSave}>
        <Text style={globalStyles.buttonText}>Create</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AddRiskLevelScreen;
