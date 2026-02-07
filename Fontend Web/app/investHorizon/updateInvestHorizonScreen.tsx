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

const UpdateInvestHorizonScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [horizon, setHorizon] = useState("");
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
        const data = await ApiService.getInvestHorizonById(Number(id));
        setHorizon(data.horizon);
      } catch {
        Alert.alert("Error", "Failed to load investment horizon.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  const handleSave = async () => {
    if (!horizon.trim()) {
      Alert.alert("Validation error", "Horizon is required.");
      return;
    }

    try {
      await ApiService.updateInvestHorizon(Number(id), { horizon });
      Alert.alert("Success", "Investment horizon updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update investment horizon.");
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Edit Investment Horizon
      </Text>

      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Horizon</Text>
        <TextInput
          style={globalStyles.input}
          value={horizon}
          onChangeText={setHorizon}
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

export default UpdateInvestHorizonScreen;
