import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Platform,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const AddCurrencyScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(2);

  const [name, setName] = useState("");
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);
  const [riskLevels, setRiskLevels] = useState<any[]>([]);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    employeeAuthGuard().then(setIsReady);
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getRiskLevels();
        setRiskLevels(data);
      } catch {
        Alert.alert("Error", "Failed to load risk levels.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleSave = async () => {
    if (!name.trim() || riskLevelId === null) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.createCurrency({
        name: name.trim(),
        currencyRiskLevelId: riskLevelId,
      });

      Alert.alert("Success", "Currency created successfully.");
      router.replace(ROUTES.CURRENCY);
    } catch {
      Alert.alert("Error", "Failed to create currency.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Currency</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Currency Name</Text>
          <TextInput
            style={globalStyles.input}
            value={name}
            onChangeText={setName}
            placeholder="e.g. USD"
            placeholderTextColor={COLORS.placeholderGrey}
          />
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Risk Level</Text>

          <View
            style={[globalStyles.pickerWrapper, globalStyles.pickerWebWrapper]}
          >
            <Picker
              selectedValue={riskLevelId}
              onValueChange={setRiskLevelId}
              style={[
                globalStyles.pickerText,
                Platform.OS === "web" && globalStyles.pickerWeb,
              ]}
            >
              <Picker.Item label="-- Select risk level --" value={null} />
              {riskLevels.map((r) => (
                <Picker.Item key={r.id} label={r.description} value={r.id} />
              ))}
            </Picker>

            {Platform.OS === "web" && (
              <Ionicons
                name="chevron-down"
                size={20}
                color={COLORS.textGrey}
                style={globalStyles.pickerWebArrow}
              />
            )}
          </View>
        </View>
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          saving && globalStyles.buttonDisabled,
        ]}
        disabled={saving}
        onPress={handleSave}
      >
        {saving ? (
          <ActivityIndicator color={COLORS.whiteHeader} />
        ) : (
          <Text style={globalStyles.buttonText}>Create Currency</Text>
        )}
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

export default AddCurrencyScreen;
