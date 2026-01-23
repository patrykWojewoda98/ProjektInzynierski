import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const EditMarketDataListScreen = () => {
  const router = useRouter();

  const [isReady, setIsReady] = useState(false);
  const [loading, setLoading] = useState(true);
  const [importing, setImporting] = useState(false);

  const [sectors, setSectors] = useState<any[]>([]);
  const [regions, setRegions] = useState<any[]>([]);
  const [countries, setCountries] = useState<any[]>([]);
  const [types, setTypes] = useState<any[]>([]);
  const [instruments, setInstruments] = useState<any[]>([]);
  const [filteredInstruments, setFilteredInstruments] = useState<any[]>([]);
  const [marketData, setMarketData] = useState<any[]>([]);

  const [sectorId, setSectorId] = useState<number | null>(null);
  const [regionId, setRegionId] = useState<number | null>(null);
  const [countryId, setCountryId] = useState<number | null>(null);
  const [typeId, setTypeId] = useState<number | null>(null);
  const [instrumentId, setInstrumentId] = useState<number | null>(null);

  const handleImportLatest = async () => {
    if (!instrumentId) return;

    setImporting(true);

    try {
      const instrument = instruments.find((i) => i.id === instrumentId);

      if (!instrument?.ticker) {
        Alert.alert("Error", "Instrument ticker not found.");
        return;
      }

      await ApiService.importMarketDataByTicker(instrument.ticker);

      const refreshed = await ApiService.getMarketDataByInstrumentId(
        instrumentId
      );
      setMarketData(refreshed);

      Alert.alert("Success", "Latest market data imported.");
    } catch {
      Alert.alert("Error", "Failed to import market data.");
    } finally {
      setImporting(false);
    }
  };

  const handleDeleteMarketData = (id: number) => {
    Alert.alert("Confirm delete", "Delete this market data record?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          try {
            await ApiService.deleteMarketData(id);
            setMarketData((p) => p.filter((x) => x.id !== id));
          } catch {
            Alert.alert("Error", "Failed to delete market data.");
          }
        },
      },
    ]);
  };

  // 🔐 AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  // 📥 LOAD FILTER DATA
  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const [s, r, c, t, i] = await Promise.all([
          ApiService.getSectors(),
          ApiService.getAllRegions(),
          ApiService.getAllCountries(),
          ApiService.getAllInvestmentTypes(),
          ApiService.getInvestInstruments(),
        ]);

        setSectors(s);
        setRegions(r);
        setCountries(c);
        setTypes(t);
        setInstruments(i);
        setFilteredInstruments(i);
      } catch {
        Alert.alert("Error", "Failed to load filters.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  // 🔎 FILTER INSTRUMENTS
  useEffect(() => {
    let data = instruments;

    if (sectorId) data = data.filter((i) => i.sectorId === sectorId);
    if (regionId) data = data.filter((i) => i.regionId === regionId);
    if (countryId) data = data.filter((i) => i.countryId === countryId);
    if (typeId) data = data.filter((i) => i.investmentTypeId === typeId);

    setFilteredInstruments(data);
    setInstrumentId(null);
    setMarketData([]);
  }, [sectorId, regionId, countryId, typeId]);

  // 📊 LOAD MARKET DATA
  useEffect(() => {
    if (!instrumentId) return;

    const loadMarketData = async () => {
      try {
        const data = await ApiService.getMarketDataByInstrumentId(instrumentId);
        setMarketData(data);
      } catch {
        Alert.alert("Error", "Failed to load market data.");
      }
    };

    loadMarketData();
  }, [instrumentId]);

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const renderPicker = (
    label: string,
    value: number | null,
    setValue: (v: number | null) => void,
    data: any[],
    labelKey = "name"
  ) => (
    <View style={[globalStyles.card, globalStyles.fullWidth]}>
      <Text style={globalStyles.label}>{label}</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          selectedValue={value}
          onValueChange={(v) => setValue(v)}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="All" value={null} />
          {data.map((x) => (
            <Picker.Item key={x.id} label={x[labelKey]} value={x.id} />
          ))}
        </Picker>
      </View>
    </View>
  );

  const formatDate = (iso: string) => iso.split("T")[0];

  const changeColor = (value: number) =>
    value > 0 ? COLORS.success : value < 0 ? COLORS.error : COLORS.textGrey;

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Market Data</Text>

      {renderPicker("Sector", sectorId, setSectorId, sectors)}
      {renderPicker("Region", regionId, setRegionId, regions)}
      {renderPicker("Country", countryId, setCountryId, countries)}
      {renderPicker("Investment Type", typeId, setTypeId, types, "typeName")}
      {renderPicker(
        "Investment Instrument",
        instrumentId,
        setInstrumentId,
        filteredInstruments,
        "ticker"
      )}

      {instrumentId && (
        <>
          <Text style={[globalStyles.header, spacing.mt4, spacing.mb2]}>
            Market Data
          </Text>
          <View style={[spacing.mt3, spacing.mb3]}>
            <TouchableOpacity
              style={globalStyles.button}
              onPress={handleImportLatest}
            >
              <Text style={globalStyles.buttonText}>
                Import latest market data
              </Text>
            </TouchableOpacity>
          </View>
          {marketData.map((m) => {
            const dailyColor =
              m.dailyChange > 0
                ? COLORS.success
                : m.dailyChange < 0
                ? COLORS.error
                : COLORS.textGrey;

            return (
              <View
                key={m.id}
                style={[globalStyles.card, globalStyles.fullWidth, spacing.mb3]}
              >
                {/* HEADER */}
                <View
                  style={[
                    globalStyles.row,
                    globalStyles.spaceBetween,
                    spacing.mb1,
                  ]}
                >
                  <Text style={globalStyles.cardTitle}>
                    {new Date(m.date).toISOString().split("T")[0]}
                  </Text>

                  <View style={globalStyles.row}>
                    <TouchableOpacity
                      style={spacing.mr3}
                      onPress={() =>
                        router.push({
                          pathname: ROUTES.EDIT_MARKET_DATA,
                          params: { id: m.id },
                        })
                      }
                    >
                      <Ionicons
                        name="pencil"
                        size={20}
                        color={COLORS.primary}
                      />
                    </TouchableOpacity>

                    <TouchableOpacity
                      onPress={() => handleDeleteMarketData(m.id)}
                    >
                      <Ionicons name="trash" size={20} color={COLORS.error} />
                    </TouchableOpacity>
                  </View>
                </View>

                {/* DETAILS */}
                <Text style={globalStyles.textSmall}>Open: {m.openPrice}</Text>
                <Text style={globalStyles.textSmall}>
                  Close: {m.closePrice}
                </Text>
                <Text style={globalStyles.textSmall}>High: {m.highPrice}</Text>
                <Text style={globalStyles.textSmall}>Low: {m.lowPrice}</Text>
                <Text style={globalStyles.textSmall}>Volume: {m.volume}</Text>

                <Text style={[globalStyles.textSmall, { color: dailyColor }]}>
                  Daily change: {m.dailyChange}
                </Text>

                <Text style={[globalStyles.textSmall, { color: dailyColor }]}>
                  Daily change %: {m.dailyChangePercent}%
                </Text>
              </View>
            );
          })}
        </>
      )}
    </ScrollView>
  );
};

export default EditMarketDataListScreen;
