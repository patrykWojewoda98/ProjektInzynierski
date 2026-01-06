import { Picker } from "@react-native-picker/picker";
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
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";
import { parseApiError } from "../../utils/apiErrorParser";

const CreateInvestInstrumentScreen = () => {
  const router = useRouter();

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

  // 📥 LOAD DICTIONARIES
  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const [r, s, t, c, cur] = await Promise.all([
          ApiService.getAllRegions(),
          ApiService.getSectors(),
          ApiService.getAllInvestmentTypes(),
          ApiService.getAllCountries(),
          ApiService.getAllCurrencies(),
        ]);

        setRegions(r);
        setSectors(s);
        setTypes(t);
        setCountries(c);
        setCurrencies(cur);
      } catch {
        Alert.alert("Error", "Failed to load dictionaries.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleCreate = async () => {
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
      await ApiService.createInvestInstrument({
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

      Alert.alert("Success", "Instrument created successfully.");
      router.replace(ROUTES.INVEST_INSTRUMENT_EDIT_LIST);
    } catch (error) {
      const messages = parseApiError(error);

      Alert.alert("Save failed", messages.join("\n"));
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
        Add Investment Instrument
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
          ["Type", investmentTypeId, setInvestmentTypeId, types, "typeName"],
          ["Country", countryId, setCountryId, countries],
          ["Currency", currencyId, setCurrencyId, currencies],
        ].map(([label, value, setter, list, nameProp]: any, i) => (
          <View key={i}>
            <Text style={globalStyles.label}>{label}</Text>
            <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
              <Picker
                selectedValue={value}
                style={globalStyles.pickerText}
                dropdownIconColor={COLORS.textGrey}
                onValueChange={(v) => setter(v)}
              >
                <Picker.Item
                  label={`-- choose ${label.toLowerCase()} --`}
                  value={null}
                />
                {list.map((x: any) => (
                  <Picker.Item
                    key={x.id}
                    label={nameProp ? x[nameProp] : x.name}
                    value={x.id}
                  />
                ))}
              </Picker>
            </View>
          </View>
        ))}
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          saving && globalStyles.buttonDisabled,
        ]}
        disabled={saving}
        onPress={handleCreate}
      >
        {saving ? (
          <ActivityIndicator color={COLORS.whiteHeader} />
        ) : (
          <Text style={globalStyles.buttonText}>Create</Text>
        )}
      </TouchableOpacity>
    </ScrollView>
  );
};

export default CreateInvestInstrumentScreen;
