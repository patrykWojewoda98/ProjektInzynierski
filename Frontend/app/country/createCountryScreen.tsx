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

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      const [r, c, rl] = await Promise.all([
        ApiService.getAllRegions(),
        ApiService.getAllCurrencies(),
        ApiService.getRiskLevels(),
      ]);
      setRegions(r);
      setCurrencies(c);
      setRiskLevels(rl);
      setLoading(false);
    };

    load();
  }, [isReady]);

  const handleSave = async () => {
    if (!name || !isoCode || !regionId || !currencyId || !riskLevelId) {
      Alert.alert("Validation error", "All fields required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.createCountry({
        name,
        isoCode,
        regionId,
        currencyId,
        countryRiskLevelId: riskLevelId,
      });

      Alert.alert("Success", "Country created.");
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

      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Name</Text>
        <TextInput
          style={globalStyles.input}
          value={name}
          onChangeText={setName}
        />

        <Text style={globalStyles.label}>ISO Code</Text>
        <TextInput
          style={globalStyles.input}
          value={isoCode}
          onChangeText={setIsoCode}
        />

        <Text style={globalStyles.label}>Region</Text>
        <Picker
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          selectedValue={regionId}
          onValueChange={setRegionId}
        >
          {regions.map((r) => (
            <Picker.Item key={r.id} label={r.name} value={r.id} />
          ))}
        </Picker>

        <Text style={globalStyles.label}>Currency</Text>
        <Picker
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          selectedValue={currencyId}
          onValueChange={setCurrencyId}
        >
          {currencies.map((c) => (
            <Picker.Item key={c.id} label={c.name} value={c.id} />
          ))}
        </Picker>

        <Text style={globalStyles.label}>Risk Level</Text>
        <Picker
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          selectedValue={riskLevelId}
          onValueChange={setRiskLevelId}
        >
          {riskLevels.map((r) => (
            <Picker.Item key={r.id} label={r.description} value={r.id} />
          ))}
        </Picker>
      </View>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mt4]}
        onPress={handleSave}
        disabled={saving}
      >
        <Text style={globalStyles.buttonText}>Save</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default CreateCountryScreen;
