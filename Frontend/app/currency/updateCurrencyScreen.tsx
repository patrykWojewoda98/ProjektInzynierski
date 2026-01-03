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
} from "react-native";
import { Picker } from "@react-native-picker/picker";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const UpdateCurrencyScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [name, setName] = useState("");
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);

  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  // 🔐 EMPLOYEE AUTH GUARD
  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  // 📥 LOAD DATA
  useEffect(() => {
    if (!id || !isReady) return;

    const loadData = async () => {
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

    loadData();
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

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* NAME */}
        <Text style={globalStyles.label}>Currency Name</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb1]}
          value={name}
          onChangeText={setName}
        />
      </View>
      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* RISK LEVEL */}
        <Text style={globalStyles.label}>Risk Level</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb1]}>
          <Picker
            selectedValue={riskLevelId}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
            onValueChange={(v) => setRiskLevelId(v)}
          >
            {riskLevels.map((r) => (
              <Picker.Item key={r.id} label={r.description} value={r.id} />
            ))}
          </Picker>
        </View>
      </View>
      {/* SAVE */}
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

      {/* ⬅️ BACK */}
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
