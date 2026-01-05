import { useLocalSearchParams, useRouter } from "expo-router";
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

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const UpdateRiskLevelScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [riskScale, setRiskScale] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  useEffect(() => {
    if (!id || !isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getRiskLevelById(Number(id));
        setRiskScale(String(data.riskScale));
        setDescription(data.description);
      } catch {
        Alert.alert("Error", "Failed to load risk level.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  const handleSave = async () => {
    if (!riskScale || !description.trim()) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    try {
      await ApiService.updateRiskLevel(Number(id), {
        riskScale: Number(riskScale),
        description,
      });
      Alert.alert("Success", "Risk level updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update risk level.");
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Risk Level</Text>

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
        <Text style={globalStyles.buttonText}>Save changes</Text>
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

export default UpdateRiskLevelScreen;
