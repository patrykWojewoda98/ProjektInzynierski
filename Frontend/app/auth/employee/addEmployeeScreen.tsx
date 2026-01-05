import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
  Switch,
} from "react-native";
import { useRouter } from "expo-router";

import ApiService from "../../../services/api";
import { globalStyles, spacing } from "../../../assets/styles/styles";
import { COLORS } from "../../../assets/Constants/colors";
import { employeeAuthGuard } from "../../../utils/employeeAuthGuard";

const AddEmployeeScreen = () => {
  const router = useRouter();
  const [isReady, setIsReady] = useState(false);

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [pesel, setPesel] = useState("");
  const [password, setPassword] = useState("");
  const [isAdmin, setIsAdmin] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  const handleSave = async () => {
    if (!name || !email || !password) {
      Alert.alert("Validation error", "Required fields missing.");
      return;
    }

    try {
      await ApiService.createEmployee({
        name,
        email,
        phoneNumber,
        pesel,
        password,
        isAdmin,
      });

      Alert.alert("Success", "Employee created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create employee.");
    }
  };

  if (!isReady) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Employee</Text>

      <View style={[globalStyles.card]}>
        <Text style={globalStyles.label}>Name</Text>
        <TextInput
          style={globalStyles.input}
          value={name}
          onChangeText={setName}
        />

        <Text style={globalStyles.label}>Email</Text>
        <TextInput
          style={globalStyles.input}
          value={email}
          onChangeText={setEmail}
        />

        <Text style={globalStyles.label}>Phone</Text>
        <TextInput
          style={globalStyles.input}
          value={phoneNumber}
          onChangeText={setPhoneNumber}
        />

        <Text style={globalStyles.label}>PESEL</Text>
        <TextInput
          style={globalStyles.input}
          value={pesel}
          onChangeText={setPesel}
        />

        <Text style={globalStyles.label}>Password</Text>
        <TextInput
          style={globalStyles.input}
          secureTextEntry
          value={password}
          onChangeText={setPassword}
        />

        <View style={[globalStyles.row, spacing.mt3]}>
          <Text style={globalStyles.text}>Admin</Text>
          <Switch value={isAdmin} onValueChange={setIsAdmin} />
        </View>
      </View>

      <TouchableOpacity style={globalStyles.button} onPress={handleSave}>
        <Text style={globalStyles.buttonText}>Create</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AddEmployeeScreen;
