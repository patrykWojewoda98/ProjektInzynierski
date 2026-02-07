import { useEffect, useState } from "react";
import {
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
} from "react-native";
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { ROUTES } from "@/routes";

const AddInvestmentTypeScreen = () => {
  const router = useRouter();
  const [typeName, setTypeName] = useState("");
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);
  const handleSave = async () => {
    if (!typeName.trim()) {
      Alert.alert("Validation", "Type name is required");
      return;
    }

    await ApiService.createInvestmentType({ typeName });
    Alert.alert("Success", "Investment type created");
    router.replace(ROUTES.INVESTMENT_TYPE);
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Add Investment Type
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
        <Text style={globalStyles.buttonText}>Save</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AddInvestmentTypeScreen;
