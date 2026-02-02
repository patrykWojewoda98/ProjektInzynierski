import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useLocalSearchParams, useRouter } from "expo-router";
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

const UpdateCurrencyScreen = () => {
  const { id } = useLocalSearchParams();
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
    if (!id || !isReady) return;

    const load = async () => {
      try {
        const [currency, risks] = await Promise.all([
          ApiService.getCurrencyByID(Number(id)),
          ApiService.getRiskLevels(),
        ]);

        setName(currency.name);
        setRiskLevelId(currency.currencyRiskLevelId);
        setRiskLevels(risks);
      } catch {
        Alert.alert("Error", "Failed to load currency.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  const handleSave = async () => {
    if (!name.trim() || riskLevelId === null) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateCurrency(Number(id), {
        id: Number(id),
        name,
        currencyRiskLevelId: riskLevelId,
      });

      Alert.alert("Success", "Currency updated successfully.");
      router.replace(ROUTES.CURRENCY);
    } catch {
      Alert.alert("Error", "Failed to update currency.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Currency</Text>

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
          />
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Risk Level</Text>

          <View
            style={[
              globalStyles.pickerWrapper,
              Platform.OS === "web" && globalStyles.pickerWebWrapper,
            ]}
          >
            <Picker
              selectedValue={riskLevelId}
              onValueChange={(v) => setRiskLevelId(v)}
              style={[
                globalStyles.pickerText,
                Platform.OS === "web" && globalStyles.pickerWeb,
              ]}
            >
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
          <Text style={globalStyles.buttonText}>Save Changes</Text>
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

export default UpdateCurrencyScreen;
