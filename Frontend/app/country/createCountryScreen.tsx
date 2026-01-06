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
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const CreateCountryScreen = () => {
  const router = useRouter();

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

  // 🔐 AUTH
  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  // 📥 LOAD DATA
  useEffect(() => {
    if (!isReady) return;

    const loadData = async () => {
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

    loadData();
  }, [isReady]);

  // 💾 SAVE
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

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Country</Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* NAME */}
        <Text style={globalStyles.label}>Name</Text>
        <TextInput
          style={globalStyles.input}
          value={name}
          onChangeText={setName}
        />

        {/* ISO CODE */}
        <Text style={globalStyles.label}>ISO Code</Text>
        <TextInput
          style={globalStyles.input}
          value={isoCode}
          onChangeText={(text) => setIsoCode(text.toUpperCase())}
          maxLength={2}
          autoCapitalize="characters"
        />

        {/* REGION */}
        <Text style={globalStyles.label}>Region</Text>
        <Picker
          selectedValue={regionId}
          onValueChange={setRegionId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="Select region" value={null} />
          {regions.map((r) => (
            <Picker.Item key={r.id} label={r.name} value={r.id} />
          ))}
        </Picker>

        {/* CURRENCY */}
        <Text style={globalStyles.label}>Currency</Text>
        <Picker
          selectedValue={currencyId}
          onValueChange={setCurrencyId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="Select currency" value={null} />
          {currencies.map((c) => (
            <Picker.Item key={c.id} label={c.name} value={c.id} />
          ))}
        </Picker>

        {/* RISK LEVEL */}
        <Text style={globalStyles.label}>Risk Level</Text>
        <Picker
          selectedValue={riskLevelId}
          onValueChange={setRiskLevelId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="Select risk level" value={null} />
          {riskLevels.map((r) => (
            <Picker.Item key={r.id} label={r.description} value={r.id} />
          ))}
        </Picker>
      </View>

      {/* SAVE */}
      <TouchableOpacity
        style={[globalStyles.button, spacing.mt4]}
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
