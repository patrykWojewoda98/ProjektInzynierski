import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
  useWindowDimensions,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const CreateInvestInstrumentScreen = () => {
  const router = useRouter();
  const { width } = useWindowDimensions();

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const columnWidth = `${100 / columns - 4}%`;

  const [name, setName] = useState("");
  const [ticker, setTicker] = useState("");
  const [description, setDescription] = useState("");
  const [isin, setIsin] = useState("");

  const [regionId, setRegionId] = useState<number | null>(null);
  const [sectorId, setSectorId] = useState<number | null>(null);
  const [investmentTypeId, setInvestmentTypeId] = useState<number | null>(null);
  const [countryId, setCountryId] = useState<number | null>(null);
  const [currencyId, setCurrencyId] = useState<number | null>(null);

  const [regions, setRegions] = useState<any[]>([]);
  const [sectors, setSectors] = useState<any[]>([]);
  const [types, setTypes] = useState<any[]>([]);
  const [countries, setCountries] = useState<any[]>([]);
  const [currencies, setCurrencies] = useState<any[]>([]);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    employeeAuthGuard().then((ok) => ok && setIsReady(true));
  }, []);

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

  const renderInput = (
    label: string,
    value: string,
    setValue: (v: string) => void,
    props: any = {},
  ) => (
    <View style={[spacing.m2, { width: columnWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>{label}</Text>
        <TextInput
          style={globalStyles.input}
          value={value}
          onChangeText={setValue}
          {...props}
        />
      </View>
    </View>
  );

  const renderPicker = (
    label: string,
    value: number | null,
    setValue: (v: number | null) => void,
    data: any[],
    labelKey = "name",
  ) => (
    <View style={[spacing.m2, { width: columnWidth }]}>
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
            <Picker.Item
              label={`-- choose ${label.toLowerCase()} --`}
              value={null}
            />
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
      <Text style={[globalStyles.header, spacing.mb4]}>
        Add Investment Instrument
      </Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {renderInput("Name", name, setName)}
        {renderInput("Ticker", ticker, setTicker, {
          autoCapitalize: "characters",
        })}
        {renderInput("Description", description, setDescription, {
          multiline: true,
        })}
        {renderInput("ISIN", isin, setIsin, {
          autoCapitalize: "characters",
        })}

        {renderPicker("Region", regionId, setRegionId, regions)}
        {renderPicker("Sector", sectorId, setSectorId, sectors)}
        {renderPicker(
          "Type",
          investmentTypeId,
          setInvestmentTypeId,
          types,
          "typeName",
        )}
        {renderPicker("Country", countryId, setCountryId, countries)}
        {renderPicker("Currency", currencyId, setCurrencyId, currencies)}
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.mt4,
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
