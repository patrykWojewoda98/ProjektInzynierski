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
  Switch,
} from "react-native";

import ApiService from "../../../services/api";
import { globalStyles, spacing } from "../../../assets/styles/styles";
import { COLORS } from "../../../assets/Constants/colors";
import { employeeAuthGuard } from "../../../utils/employeeAuthGuard";

const UpdateEmployeeScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [pesel, setPesel] = useState("");
  const [isAdmin, setIsAdmin] = useState(false);

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
        const e = await ApiService.getEmployeeById(Number(id));
        setName(e.name);
        setEmail(e.email);
        setPhoneNumber(e.phoneNumber);
        setPesel(e.pesel);
        setIsAdmin(e.isAdmin);
      } catch {
        Alert.alert("Error", "Failed to load employee.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  const handleSave = async () => {
    try {
      await ApiService.updateEmployee(Number(id), {
        name,
        email,
        phoneNumber,
        pesel,
        isAdmin,
      });

      Alert.alert("Success", "Employee updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update employee.");
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Employee</Text>

      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Name</Text>
        <TextInput
          style={globalStyles.input}
          value={name}
          onChangeText={setName}
        />

        <Text style={globalStyles.label}>Email</Text>
        <TextInput style={globalStyles.input} value={email} editable={false} />

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

        <View style={[globalStyles.row, spacing.mt3]}>
          <Text style={globalStyles.text}>Admin</Text>
          <Switch value={isAdmin} onValueChange={setIsAdmin} />
        </View>
      </View>

      <TouchableOpacity style={globalStyles.button} onPress={handleSave}>
        <Text style={globalStyles.buttonText}>Save changes</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default UpdateEmployeeScreen;
