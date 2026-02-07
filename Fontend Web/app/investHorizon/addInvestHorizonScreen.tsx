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

const AddInvestHorizonScreen = () => {
  const router = useRouter();
  const [horizon, setHorizon] = useState("");
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  const handleSave = async () => {
    if (!horizon.trim()) {
      Alert.alert("Validation error", "Horizon is required.");
      return;
    }

    try {
      await ApiService.createInvestHorizon({ horizon });
      Alert.alert("Success", "Investment horizon created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create investment horizon.");
    }
  };

  if (!isReady) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Add Investment Horizon
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
        <Text style={globalStyles.buttonText}>Create</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AddInvestHorizonScreen;
