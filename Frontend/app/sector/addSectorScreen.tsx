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

const AddSectorScreen = () => {
  const router = useRouter();

  const [name, setName] = useState("");
  const [code, setCode] = useState("");
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
    if (!name.trim() || !code.trim() || !description.trim()) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    try {
      await ApiService.createSector({ name, code, description });
      Alert.alert("Success", "Sector created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create sector.");
    }
  };

  if (!isReady) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Sector</Text>

      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Name</Text>
        <TextInput
          style={globalStyles.input}
          value={name}
          onChangeText={setName}
        />

        <Text style={globalStyles.label}>Code</Text>
        <TextInput
          style={globalStyles.input}
          value={code}
          onChangeText={setCode}
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

export default AddSectorScreen;
