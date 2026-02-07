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
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const CreateCountryScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

  const [name, setName] = useState("");
  const [isoCode, setIsoCode] = useState("");

  const [regionId, setRegionId] = useState<number | null>(null);
  const [currencyId, setCurrencyId] = useState<number | null>(null);
  const [riskLevelId, setRiskLevelId] = useState<number | null>(null);

  const [regions, setRegions] = useState<any[]>([]);
  const [currencies, setCurrencies] = useState<any[]>([]);
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
        const [r, c, rl] = await Promise.all([
          ApiService.getAllRegions(),
          ApiService.getAllCurrencies(),
          ApiService.getRiskLevels(),
        ]);

        setRegions(r);
        setCurrencies(c);
        setRiskLevels(rl);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleSave = async () => {
    if (!name || !isoCode || !regionId || !currencyId || !riskLevelId) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.createCountry({
        name: name.trim(),
        isoCode: isoCode.trim().toUpperCase(),
        regionId,
        currencyId,
        countryRiskLevelId: riskLevelId,
      });

      Alert.alert("Success", "Country created successfully.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create country.");
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
    labelKey = "name",
    placeholder = "Select",
  ) => (
    <View style={[spacing.m2, { width: itemWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>{label}</Text>

        <View
          style={[globalStyles.pickerWrapper, globalStyles.pickerWebWrapper]}
        >
          <Picker
            selectedValue={value}
            onValueChange={(v) => setValue(v)}
            style={[
              globalStyles.pickerText,
              Platform.OS === "web" && globalStyles.pickerWeb,
            ]}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label={placeholder} value={null} />
            {data.map((x) => (
              <Picker.Item
                key={x.id}
                label={x[labelKey] ?? x.name ?? ""}
                value={x.id}
              />
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
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Country</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[spacing.m2, { width: itemWidth }]}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>Name</Text>
            <TextInput
              style={globalStyles.input}
              value={name}
              onChangeText={setName}
            />
          </View>
        </View>

        <View style={[spacing.m2, { width: itemWidth }]}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>ISO Code</Text>
            <TextInput
              style={globalStyles.input}
              value={isoCode}
              onChangeText={(v) => setIsoCode(v.toUpperCase())}
              maxLength={2}
              autoCapitalize="characters"
            />
          </View>
        </View>

        {renderPicker(
          "Region",
          regionId,
          setRegionId,
          regions,
          "name",
          "Select region",
        )}
        {renderPicker(
          "Currency",
          currencyId,
          setCurrencyId,
          currencies,
          "name",
          "Select currency",
        )}
        {renderPicker(
          "Risk Level",
          riskLevelId,
          setRiskLevelId,
          riskLevels,
          "description",
          "Select risk level",
        )}
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          saving && globalStyles.buttonDisabled,
          spacing.mt4,
        ]}
        onPress={handleSave}
        disabled={saving}
      >
        {saving ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Save</Text>
        )}
      </TouchableOpacity>
    </ScrollView>
  );
};

export default CreateCountryScreen;
