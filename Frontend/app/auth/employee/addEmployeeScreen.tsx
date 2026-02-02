import { useRouter } from "expo-router";
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
import { showValidationErrors } from "../../../utils/showValidationErrors";
import { useResponsiveColumns } from "../../../utils/useResponsiveColumns";

const AddEmployeeScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

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
    } catch (error) {
      Alert.alert("Validation error", showValidationErrors(error));
    }
  };

  if (!isReady) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Employee</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {[
          ["Name", name, setName, false],
          ["Email", email, setEmail, false],
          ["Phone", phoneNumber, setPhoneNumber, false],
          ["PESEL", pesel, setPesel, false],
          ["Password", password, setPassword, true],
        ].map(([label, value, setter, secure], i) => (
          <View key={i} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
              <Text style={globalStyles.label}>{label}</Text>
              <TextInput
                style={globalStyles.input}
                value={value as string}
                onChangeText={setter as any}
                secureTextEntry={secure as boolean}
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
        <Text style={globalStyles.buttonText}>Create</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AddEmployeeScreen;
