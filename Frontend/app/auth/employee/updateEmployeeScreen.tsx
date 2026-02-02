import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Switch,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../../assets/Constants/colors";
import { globalStyles, spacing } from "../../../assets/styles/styles";
import ApiService from "../../../services/api";
import { employeeAuthGuard } from "../../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../../utils/useResponsiveColumns";

const UpdateEmployeeScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

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

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {[
          ["Name", name, setName, true],
          ["Email", email, () => {}, false],
          ["Phone", phoneNumber, setPhoneNumber, true],
          ["PESEL", pesel, setPesel, true],
        ].map(([label, value, setter, editable], i) => (
          <View key={i} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
              <Text style={globalStyles.label}>{label}</Text>
              <TextInput
                style={globalStyles.input}
                value={value as string}
                onChangeText={setter as any}
                editable={editable as boolean}
              />
            </View>
          </View>
        ))}

        <View style={[spacing.m2, { width: itemWidth }]}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>Admin</Text>
            <View
              style={[
                globalStyles.row,
                globalStyles.spaceBetween,
                { alignItems: "center" },
              ]}
            >
              <Text style={globalStyles.text}>
                Grant administrator privileges
              </Text>
              <Switch
                value={isAdmin}
                onValueChange={setIsAdmin}
                thumbColor={isAdmin ? COLORS.primary : undefined}
              />
            </View>
          </View>
        </View>
      </View>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mt4]}
        onPress={handleSave}
      >
        <Text style={globalStyles.buttonText}>Save changes</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default UpdateEmployeeScreen;
