import { Picker } from "@react-native-picker/picker";
import { useEffect, useMemo, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { authGuard } from "@/utils/authGuard";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const MarketDataListScreen = () => {
  const [isReady, setIsReady] = useState(false);
  const [loading, setLoading] = useState(true);
  const [importing, setImporting] = useState(false);

  const [sectors, setSectors] = useState<any[]>([]);
  const [regions, setRegions] = useState<any[]>([]);
  const [countries, setCountries] = useState<any[]>([]);
  const [types, setTypes] = useState<any[]>([]);
  const [instruments, setInstruments] = useState<any[]>([]);
  const [marketData, setMarketData] = useState<any[]>([]);

  const [sectorId, setSectorId] = useState<number>(0);
  const [regionId, setRegionId] = useState<number>(0);
  const [countryId, setCountryId] = useState<number>(0);
  const [typeId, setTypeId] = useState<number>(0);
  const [instrumentId, setInstrumentId] = useState<number>(0);

  useEffect(() => {
    const check = async () => {
      const ok = await authGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      setLoading(true);
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
      } catch {
        Alert.alert("Error", "Failed to load filters.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const filteredInstruments = useMemo(() => {
    let data = instruments;

    if (sectorId > 0) data = data.filter((i) => i.sectorId === sectorId);
    if (regionId > 0) data = data.filter((i) => i.regionId === regionId);
    if (countryId > 0) data = data.filter((i) => i.countryId === countryId);
    if (typeId > 0) data = data.filter((i) => i.investmentTypeId === typeId);

    return data;
  }, [sectorId, regionId, countryId, typeId, instruments]);

  useEffect(() => {
    setInstrumentId(0);
    setMarketData([]);
  }, [sectorId, regionId, countryId, typeId]);

  useEffect(() => {
    if (instrumentId === 0) return;

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

  const handleImportLatest = async () => {
    if (instrumentId === 0) return;

    setImporting(true);

    try {
      const instrument = instruments.find((i) => i.id === instrumentId);

      if (!instrument?.ticker) {
        Alert.alert("Error", "Instrument ticker not found.");
        return;
      }

      await ApiService.importMarketDataByTicker(instrument.ticker);

      const refreshed =
        await ApiService.getMarketDataByInstrumentId(instrumentId);

      setMarketData(refreshed);
      Alert.alert("Success", "Latest market data imported.");
    } catch {
      Alert.alert("Error", "Failed to import market data.");
    } finally {
      setImporting(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const renderPicker = (
    label: string,
    value: number,
    setValue: (v: number) => void,
    data: any[],
    labelKey = "name",
  ) => (
    <View style={[globalStyles.card, globalStyles.fullWidth]}>
      <Text style={globalStyles.label}>{label}</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          selectedValue={value}
          onValueChange={(v) => setValue(Number(v))}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="All" value={0} />
          {data.map((x) => (
            <Picker.Item key={x.id} label={x[labelKey]} value={x.id} />
          ))}
        </Picker>
      </View>
    </View>
  );

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
        "ticker",
      )}

      {instrumentId > 0 && (
        <>
          <View style={spacing.mt3}>
            <TouchableOpacity
              style={[
                globalStyles.button,
                importing && globalStyles.buttonDisabled,
              ]}
              disabled={importing}
              onPress={handleImportLatest}
            >
              {importing ? (
                <ActivityIndicator color="#fff" />
              ) : (
                <Text style={globalStyles.buttonText}>
                  Import latest market data
                </Text>
              )}
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
                <Text style={globalStyles.cardTitle}>
                  {new Date(m.date).toISOString().split("T")[0]}
                </Text>

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

export default MarketDataListScreen;
