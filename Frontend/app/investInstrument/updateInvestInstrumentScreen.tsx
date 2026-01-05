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

import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";

const UpdateInvestInstrumentScreen = () => {
  const router = useRouter();
  const { id } = useLocalSearchParams();

  // 🔹 BASIC
  const [name, setName] = useState("");
  const [ticker, setTicker] = useState("");
  const [description, setDescription] = useState("");
  const [isin, setIsin] = useState("");

  // 🔹 IDS
  const [regionId, setRegionId] = useState<number | null>(null);
  const [sectorId, setSectorId] = useState<number | null>(null);
  const [investmentTypeId, setInvestmentTypeId] = useState<number | null>(null);
  const [countryId, setCountryId] = useState<number | null>(null);
  const [currencyId, setCurrencyId] = useState<number | null>(null);

  // 🔹 LISTS
  const [regions, setRegions] = useState<any[]>([]);
  const [sectors, setSectors] = useState<any[]>([]);
  const [types, setTypes] = useState<any[]>([]);
  const [countries, setCountries] = useState<any[]>([]);
  const [currencies, setCurrencies] = useState<any[]>([]);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  // 🔐 AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  // 📥 LOAD DATA
  useEffect(() => {
    if (!isReady || !id) return;

    const load = async () => {
      try {
        const [instrument, r, s, t, c, cur, m] = await Promise.all([
          ApiService.getInvestInstrumentById(Number(id)),
          ApiService.getAllRegions(),
          ApiService.getSectors(),
          ApiService.getAllInvestmentTypes(),
          ApiService.getAllCountries(),
          ApiService.getAllCurrencies(),
        ]);

        // lists
        setRegions(r);
        setSectors(s);
        setTypes(t);
        setCountries(c);
        setCurrencies(cur);

        // values
        setName(instrument.name ?? "");
        setTicker(instrument.ticker ?? "");
        setDescription(instrument.description ?? "");
        setIsin(instrument.isin ?? "");

        setRegionId(instrument.regionId ?? null);
        setSectorId(instrument.sectorId ?? null);
        setInvestmentTypeId(instrument.investmentTypeId ?? null);
        setCountryId(instrument.countryId ?? null);
        setCurrencyId(instrument.currencyId ?? null);
      } catch {
        Alert.alert("Error", "Failed to load instrument.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady, id]);

  // 💾 SAVE
  const handleSave = async () => {
    if (
      !name.trim() ||
      !ticker.trim() ||
      !regionId ||
      !sectorId ||
      !investmentTypeId ||
      !countryId ||
      !currencyId
    ) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateInvestInstrument(Number(id), {
        id: Number(id),
        name,
        ticker,
        description,
        isin,
        regionId,
        sectorId,
        investmentTypeId,
        countryId,
        currencyId,
        marketDataDate: new Date().toISOString(),
      });

      Alert.alert("Success", "Instrument updated successfully.");
      router.replace(ROUTES.INVEST_INSTRUMENT_EDIT_LIST);
    } catch {
      Alert.alert("Error", "Update failed.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Edit Investment Instrument
      </Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <Text style={globalStyles.label}>Name</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={name}
          onChangeText={setName}
        />

        <Text style={globalStyles.label}>Ticker</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={ticker}
          onChangeText={setTicker}
          autoCapitalize="characters"
        />

        <Text style={globalStyles.label}>Description</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={description}
          onChangeText={setDescription}
          multiline
        />

        <Text style={globalStyles.label}>ISIN</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={isin}
          onChangeText={setIsin}
          autoCapitalize="characters"
        />

        {/* PICKERS */}
        {[
          ["Region", regionId, setRegionId, regions],
          ["Sector", sectorId, setSectorId, sectors],
          ["Country", countryId, setCountryId, countries],
          ["Currency", currencyId, setCurrencyId, currencies],
        ].map(([label, value, setter, list]: any, i) => (
          <View key={i}>
            <Text style={globalStyles.label}>{label}</Text>
            <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
              <Picker
                selectedValue={value}
                style={globalStyles.pickerText}
                dropdownIconColor={COLORS.textGrey}
                onValueChange={(v) => setter(v)}
              >
                {list.map((x: any) => (
                  <Picker.Item key={x.id} label={x.name} value={x.id} />
                ))}
              </Picker>
            </View>
          </View>
        ))}
        <Text style={globalStyles.label}>Investment Type</Text>
        <Picker
          selectedValue={investmentTypeId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          onValueChange={(v) => setInvestmentTypeId(v)}
        >
          <Picker.Item label="-- choose type --" value={null} />
          {types.map((t) => (
            <Picker.Item
              key={t.id}
              label={t.typeName ?? t.name ?? "Type"} // ✅ TU JEST FIX
              value={t.id}
            />
          ))}
        </Picker>
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
    </ScrollView>
  );
};

export default UpdateInvestInstrumentScreen;
