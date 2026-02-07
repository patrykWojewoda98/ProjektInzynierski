import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
} from "react-native";

import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";

const UpdateInvestmentTypeScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [typeName, setTypeName] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    employeeAuthGuard();

    ApiService.getInvestmentTypeById(Number(id))
      .then((d) => setTypeName(d.typeName))
      .catch(() => Alert.alert("Error", "Failed to load"))
      .finally(() => setLoading(false));
  }, []);

  const handleSave = async () => {
    await ApiService.updateInvestmentType(Number(id), { typeName });
    Alert.alert("Success", "Updated");
    router.back();
  };

  if (loading) return <ActivityIndicator color={COLORS.primary} />;

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Edit Investment Type
      </Text>

      <Text style={globalStyles.label}>Type name</Text>
      <TextInput
        style={globalStyles.input}
        value={typeName}
        onChangeText={setTypeName}
      />

      <TouchableOpacity
        style={[globalStyles.button, spacing.mt4]}
        onPress={handleSave}
      >
        <Text style={globalStyles.buttonText}>Save changes</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default UpdateInvestmentTypeScreen;
