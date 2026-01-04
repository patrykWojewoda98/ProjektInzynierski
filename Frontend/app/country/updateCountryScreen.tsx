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

const UpdateCountryScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

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

  // 🔐 EMPLOYEE AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  // 📥 LOAD DATA
  useEffect(() => {
    if (!id || !isReady) return;

    const load = async () => {
      try {
        const [country, regionData, currencyData, riskData] = await Promise.all(
          [
            ApiService.getCountryById(Number(id)),
            ApiService.getAllRegions(),
            ApiService.getAllCurrencies(),
            ApiService.getRiskLevels(),
          ]
        );

        setCountryName(country.name);
        setIsoCode(country.isoCode);
        setRegionId(country.regionId);
        setCurrencyId(country.currencyId);
        setRiskLevelId(country.countryRiskLevelId);

        setRegions(regionData);
        setCurrencies(currencyData);
        setRiskLevels(riskData);
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
        isoCode,
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

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Country</Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* NAME */}
        <Text style={globalStyles.label}>Country Name</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={countryName}
          onChangeText={setCountryName}
        />

        {/* ISO */}
        <Text style={globalStyles.label}>ISO Code</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={isoCode}
          onChangeText={setIsoCode}
          autoCapitalize="characters"
        />

        {/* REGION */}
        <Text style={globalStyles.label}>Region</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
          <Picker
            selectedValue={regionId}
            onValueChange={(v) => setRegionId(v)}
            style={globalStyles.pickerText}
          >
            {regions.map((r) => (
              <Picker.Item key={r.id} label={r.name} value={r.id} />
            ))}
          </Picker>
        </View>

        {/* CURRENCY */}
        <Text style={globalStyles.label}>Currency</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
          <Picker
            selectedValue={currencyId}
            onValueChange={(v) => setCurrencyId(v)}
            style={globalStyles.pickerText}
          >
            {currencies.map((c) => (
              <Picker.Item key={c.id} label={c.name} value={c.id} />
            ))}
          </Picker>
        </View>

        {/* RISK */}
        <Text style={globalStyles.label}>Risk Level</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb4]}>
          <Picker
            selectedValue={riskLevelId}
            onValueChange={(v) => setRiskLevelId(v)}
            style={globalStyles.pickerText}
          >
            {riskLevels.map((r) => (
              <Picker.Item key={r.id} label={r.description} value={r.id} />
            ))}
          </Picker>
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
      </View>

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

export default UpdateCountryScreen;
