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

const UpdateRegionScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(3);

  const [name, setName] = useState("");
  const [regionCodeId, setRegionCodeId] = useState<number | null>(null);
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);

  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [regionCodes, setRegionCodes] = useState<any[]>([]);

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
        const [region, risks, codes] = await Promise.all([
          ApiService.getRegionById(Number(id)),
          ApiService.getRiskLevels(),
          ApiService.getAllRegionCodes(),
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

    load();
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
        name: name.trim(),
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

  const renderPicker = (
    label: string,
    value: number | null,
    setValue: (v: number | null) => void,
    data: any[],
    labelKey: string,
    emptyLabel?: string,
  ) => (
    <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
      <Text style={globalStyles.label}>{label}</Text>

      <View style={[globalStyles.pickerWrapper, globalStyles.pickerWebWrapper]}>
        <Picker
          selectedValue={value}
          onValueChange={setValue}
          style={[
            globalStyles.pickerText,
            Platform.OS === "web" && globalStyles.pickerWeb,
          ]}
        >
          {emptyLabel && <Picker.Item label={emptyLabel} value={null} />}
          {data.map((x) => (
            <Picker.Item key={x.id} label={x[labelKey]} value={x.id} />
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
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Region</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Region Name</Text>
          <TextInput
            style={globalStyles.input}
            value={name}
            onChangeText={setName}
          />
        </View>

        {renderPicker(
          "Region Code",
          regionCodeId,
          setRegionCodeId,
          regionCodes,
          "code",
          "— No code —",
        )}

        {renderPicker(
          "Risk Level",
          riskLevelId,
          setRiskLevelId,
          riskLevels,
          "description",
        )}
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

export default UpdateRegionScreen;
