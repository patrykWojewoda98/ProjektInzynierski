import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
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

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const AddRegionScreen = () => {
  const router = useRouter();

  const [name, setName] = useState("");
  const [regionCodeId, setRegionCodeId] = useState<number | null>(null);
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);

  const [regionCodes, setRegionCodes] = useState<any[]>([]);
  const [riskLevels, setRiskLevels] = useState<any[]>([]);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const loadData = async () => {
      try {
        const [codes, risks] = await Promise.all([
          ApiService.getAllRegionCodes(),
          ApiService.getRiskLevels(),
        ]);

        setRegionCodes(codes);
        setRiskLevels(risks);
      } catch {
        Alert.alert("Error", "Failed to load dictionaries.");
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [isReady]);

  const handleSave = async () => {
    if (!name.trim() || !riskLevelId) {
      Alert.alert("Validation error", "Name and risk level are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.createRegion({
        name,
        regionCodeId,
        regionRiskLevelId: riskLevelId,
      });

      Alert.alert("Success", "Region created successfully.");
      router.replace(ROUTES.REGION);
    } catch {
      Alert.alert("Error", "Failed to create region.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Region</Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* NAME */}
        <Text style={globalStyles.label}>Region Name</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={name}
          onChangeText={setName}
          placeholder="Region name"
          placeholderTextColor={COLORS.placeholderGrey}
        />

        {/* REGION CODE */}
        <Text style={globalStyles.label}>Region Code</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
          <Picker
            selectedValue={regionCodeId}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
            onValueChange={(v) => setRegionCodeId(v)}
          >
            <Picker.Item label="-- No code --" value={null} />
            {regionCodes.map((c) => (
              <Picker.Item key={c.id} label={c.code} value={c.id} />
            ))}
          </Picker>
        </View>

        {/* RISK LEVEL */}
        <Text style={globalStyles.label}>Risk Level</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb4]}>
          <Picker
            selectedValue={riskLevelId}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
            onValueChange={(v) => setRiskLevelId(v)}
          >
            <Picker.Item label="-- Choose risk level --" value={null} />
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
          <Text style={globalStyles.buttonText}>Save</Text>
        )}
      </TouchableOpacity>

      {/* BACK */}
      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default AddRegionScreen;
