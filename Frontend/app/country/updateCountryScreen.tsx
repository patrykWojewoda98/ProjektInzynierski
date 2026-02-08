import { Picker } from "@react-native-picker/picker";
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

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const UpdateCountryScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

  const [countryName, setCountryName] = useState("");
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
    if (!id || !isReady) return;

    const load = async () => {
      try {
        const [country, r, c, rl] = await Promise.all([
          ApiService.getCountryById(Number(id)),
          ApiService.getAllRegions(),
          ApiService.getAllCurrencies(),
          ApiService.getRiskLevels(),
        ]);

        setCountryName(country.name);
        setIsoCode(country.isoCode);
        setRegionId(country.regionId);
        setCurrencyId(country.currencyId);
        setRiskLevelId(country.countryRiskLevelId);

        setRegions(r);
        setCurrencies(c);
        setRiskLevels(rl);
      } catch {
        Alert.alert("Error", "Failed to load country.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  const handleSave = async () => {
    if (
      !countryName.trim() ||
      !isoCode.trim() ||
      regionId === null ||
      currencyId === null ||
      riskLevelId === null
    ) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateCountry(Number(id), {
        id: Number(id),
        name: countryName.trim(),
        isoCode: isoCode.trim().toUpperCase(),
        regionId,
        currencyId,
        countryRiskLevelId: riskLevelId,
      });
      Alert.alert("Success", "Country updated successfully.");
      router.replace(ROUTES.COUNTRY);
    } catch {
      Alert.alert("Error", "Failed to update country.");
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
  ) => (
    <View style={[spacing.m2, { width: itemWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>{label}</Text>
        <View
          style={[
            globalStyles.pickerWrapper,
            globalStyles.pickerWebWrapper,
            {
              flexDirection: "row",
              alignItems: "center",
              paddingHorizontal: 12,
              height: 48,
            },
          ]}
        >
          <Picker
            selectedValue={value}
            onValueChange={(v) => setValue(v)}
            style={[
              globalStyles.pickerText,
              globalStyles.pickerWeb,
              { flex: 1 },
            ]}
            dropdownIconColor={COLORS.textGrey}
          >
            {data.map((x) => (
              <Picker.Item
                key={x.id}
                label={x[labelKey] ?? x.name}
                value={x.id}
              />
            ))}
          </Picker>
        </View>
      </View>
    </View>
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Country</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[spacing.m2, { width: itemWidth }]}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>Country Name</Text>
            <TextInput
              style={globalStyles.input}
              value={countryName}
              onChangeText={setCountryName}
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
              autoCapitalize="characters"
              maxLength={2}
            />
          </View>
        </View>

        {renderPicker("Region", regionId, setRegionId, regions)}
        {renderPicker("Currency", currencyId, setCurrencyId, currencies)}
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
          saving && globalStyles.buttonDisabled,
          spacing.mt4,
        ]}
        disabled={saving}
        onPress={handleSave}
      >
        {saving ? (
          <ActivityIndicator color="#fff" />
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

export default UpdateCountryScreen;
