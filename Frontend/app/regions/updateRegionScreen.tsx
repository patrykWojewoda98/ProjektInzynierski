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

const UpdateRegionScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [name, setName] = useState("");
  const [regionCodeId, setRegionCodeId] = useState<number | null>(null);
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);

  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [regionCodes, setRegionCodes] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  // 🔐 AUTH
  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  // 📥 LOAD
  useEffect(() => {
    if (!id || !isReady) return;

    const loadData = async () => {
      try {
        const [region, risks, codes] = await Promise.all([
          ApiService.getRegionById(Number(id)),
          ApiService.getRiskLevels(),
          ApiService.getRegionCodes(),
        ]);

        setName(region.name);
        setRiskLevelId(region.regionRiskLevelId);
        setRegionCodeId(region.regionCodeId ?? null);
        setRiskLevels(risks);
        setRegionCodes(codes);
      } catch {
        Alert.alert("Error", "Failed to load region.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [id, isReady]);

  const handleSave = async () => {
    if (!name.trim() || riskLevelId === null) {
      Alert.alert("Validation error", "Name and risk level are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateRegion(Number(id), {
        id: Number(id),
        name,
        regionCodeId,
        regionRiskLevelId: riskLevelId,
      });

      Alert.alert("Success", "Region updated successfully.");
      router.replace(ROUTES.REGION);
    } catch {
      Alert.alert("Error", "Failed to update region.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Region</Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <Text style={globalStyles.label}>Region Name</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={name}
          onChangeText={setName}
        />
      </View>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <Text style={globalStyles.label}>Region Code</Text>
        <View style={globalStyles.pickerWrapper}>
          <Picker
            selectedValue={regionCodeId}
            onValueChange={(v) => setRegionCodeId(v)}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="— No code —" value={null} />
            {regionCodes.map((c) => (
              <Picker.Item key={c.id} label={c.code} value={c.id} />
            ))}
          </Picker>
        </View>
      </View>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <Text style={globalStyles.label}>Risk Level</Text>
        <View style={globalStyles.pickerWrapper}>
          <Picker
            selectedValue={riskLevelId}
            onValueChange={(v) => setRiskLevelId(v)}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
          >
            {riskLevels.map((r) => (
              <Picker.Item key={r.id} label={r.description} value={r.id} />
            ))}
          </Picker>
        </View>
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          saving && globalStyles.buttonDisabled,
        ]}
        onPress={handleSave}
        disabled={saving}
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

export default UpdateRegionScreen;
