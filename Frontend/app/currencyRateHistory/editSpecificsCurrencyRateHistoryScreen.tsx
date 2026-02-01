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
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

type CurrencyRateHistoryResponse = {
  id: number;
  currencyPairId: number;
  date: string;
  openRate: number | null;
  highRate: number | null;
  lowRate: number | null;
  closeRate: number;
};

const EditSpecificsCurrencyRateHistoryScreen = () => {
  const router = useRouter();
  const { rateId } = useLocalSearchParams();

  const parsedRateId = Number(Array.isArray(rateId) ? rateId[0] : rateId);

  const [date, setDate] = useState("");

  const [openRate, setOpenRate] = useState("");
  const [highRate, setHighRate] = useState("");
  const [lowRate, setLowRate] = useState("");
  const [closeRate, setCloseRate] = useState("");

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);
  const [symbol, setSymbol] = useState<string | null>(null);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    if (!parsedRateId || Number.isNaN(parsedRateId)) {
      Alert.alert("Error", "Invalid rate id.");
      setLoading(false);
      router.back();
      return;
    }

    const load = async () => {
      try {
        const data: CurrencyRateHistoryResponse =
          await ApiService.getCurrencyRateHistoryById(parsedRateId);

        setDate(data.date.split("T")[0]);

        setOpenRate(data.openRate?.toString() ?? "");
        setHighRate(data.highRate?.toString() ?? "");
        setLowRate(data.lowRate?.toString() ?? "");
        setCloseRate(data.closeRate.toString());

        const pair = await ApiService.getCurrencyPairById(data.currencyPairId);
        setSymbol(pair.symbol);
      } catch {
        Alert.alert("Error", "Failed to load currency rate.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady, parsedRateId]);

  const parseNullableDecimal = (value: string) =>
    value === "" ? null : Number(value);

  const handleSave = async () => {
    if (!date || !/^\d{4}-\d{2}-\d{2}$/.test(date)) {
      Alert.alert("Validation error", "Date must be YYYY-MM-DD.");
      return;
    }

    const parsedCloseRate = Number(closeRate);

    if (Number.isNaN(parsedCloseRate)) {
      Alert.alert("Validation error", "Close rate must be a number.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateCurrencyRateHistory(parsedRateId, {
        date,
        openRate: parseNullableDecimal(openRate),
        highRate: parseNullableDecimal(highRate),
        lowRate: parseNullableDecimal(lowRate),
        closeRate: parsedCloseRate,
      });

      Alert.alert("Success", "Currency rate updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update currency rate.");
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
        Edit Currency Rate {symbol ? `– ${symbol}` : ""}
      </Text>

      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Date</Text>
        <TextInput
          style={globalStyles.input}
          value={date}
          onChangeText={setDate}
          placeholder="YYYY-MM-DD"
        />

        <Text style={globalStyles.label}>Open</Text>
        <TextInput
          style={globalStyles.input}
          value={openRate}
          onChangeText={setOpenRate}
          keyboardType="numeric"
        />

        <Text style={globalStyles.label}>High</Text>
        <TextInput
          style={globalStyles.input}
          value={highRate}
          onChangeText={setHighRate}
          keyboardType="numeric"
        />

        <Text style={globalStyles.label}>Low</Text>
        <TextInput
          style={globalStyles.input}
          value={lowRate}
          onChangeText={setLowRate}
          keyboardType="numeric"
        />

        <Text style={globalStyles.label}>Close</Text>
        <TextInput
          style={globalStyles.input}
          value={closeRate}
          onChangeText={setCloseRate}
          keyboardType="numeric"
        />
      </View>

      <TouchableOpacity
        style={[globalStyles.button, saving && globalStyles.buttonDisabled]}
        disabled={saving}
        onPress={handleSave}
      >
        {saving ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Save changes</Text>
        )}
      </TouchableOpacity>

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default EditSpecificsCurrencyRateHistoryScreen;
