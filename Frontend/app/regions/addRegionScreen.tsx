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

const AddRegionScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(3);

  const [name, setName] = useState("");
  const [regionCodeId, setRegionCodeId] = useState<number | null>(null);
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);

  const [regionCodes, setRegionCodes] = useState<any[]>([]);
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

    load();
  }, [isReady]);

  const handleSave = async () => {
    if (!name.trim() || !riskLevelId) {
      Alert.alert("Validation error", "Name and risk level are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.createRegion({
        name: name.trim(),
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

  const renderPicker = (
    label: string,
    value: number | null,
    setValue: (v: number | null) => void,
    data: any[],
    labelKey: string,
    emptyLabel: string,
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
          <Picker.Item label={emptyLabel} value={null} />
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
      <Text style={[globalStyles.header, spacing.mb4]}>Add Region</Text>

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
            placeholder="Region name"
            placeholderTextColor={COLORS.placeholderGrey}
          />
        </View>

        {renderPicker(
          "Region Code",
          regionCodeId,
          setRegionCodeId,
          regionCodes,
          "code",
          "-- No code --",
        )}

        {renderPicker(
          "Risk Level",
          riskLevelId,
          setRiskLevelId,
          riskLevels,
          "description",
          "-- Choose risk level --",
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
          <Text style={globalStyles.buttonText}>Save</Text>
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

export default AddRegionScreen;
