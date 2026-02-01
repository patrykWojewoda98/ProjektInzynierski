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
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const UpdateCurrencyPairScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [currencies, setCurrencies] = useState<any[]>([]);

  const [baseCurrencyId, setBaseCurrencyId] = useState<number>(0);
  const [quoteCurrencyId, setQuoteCurrencyId] = useState<number>(0);
  const [symbol, setSymbol] = useState("");

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!id || !isReady) return;

    const load = async () => {
      try {
        const [pair, currencyList] = await Promise.all([
          ApiService.getCurrencyPairById(Number(id)),
          ApiService.getAllCurrencies(),
        ]);

        setCurrencies(currencyList);
        setBaseCurrencyId(pair.baseCurrencyId);
        setQuoteCurrencyId(pair.quoteCurrencyId);
        setSymbol(pair.symbol);
      } catch {
        Alert.alert("Error", "Failed to load currency pair.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  useEffect(() => {
    if (baseCurrencyId > 0 && quoteCurrencyId > 0) {
      const base = currencies.find((c) => c.id === baseCurrencyId);
      const quote = currencies.find((c) => c.id === quoteCurrencyId);

      if (base && quote) {
        setSymbol(`${base.name}/${quote.name}`);
      }
    }
  }, [baseCurrencyId, quoteCurrencyId, currencies]);

  const currencyPairId = Array.isArray(id) ? Number(id[0]) : Number(id);

  const handleSave = async () => {
    if (!currencyPairId) {
      Alert.alert("Error", "Invalid currency pair ID.");
      return;
    }

    if (baseCurrencyId === 0 || quoteCurrencyId === 0) {
      Alert.alert("Validation error", "Select both currencies.");
      return;
    }

    if (baseCurrencyId === quoteCurrencyId) {
      Alert.alert(
        "Validation error",
        "Base currency and quote currency must be different.",
      );
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateCurrencyPair(currencyPairId, {
        id: currencyPairId,
        baseCurrencyId,
        quoteCurrencyId,
        symbol: symbol.trim(),
      });

      Alert.alert("Success", "Currency pair updated.");
      router.back();
    } catch (err) {
      console.log(err);
      Alert.alert("Error", "Failed to update currency pair.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const renderCurrencyPicker = (
    label: string,
    value: number,
    setValue: (v: number) => void,
  ) => (
    <>
      <Text style={globalStyles.label}>{label}</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          selectedValue={value}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          onValueChange={(v) => setValue(Number(v))}
        >
          <Picker.Item label="Select currency" value={0} />
          {currencies.map((c) => (
            <Picker.Item key={c.id} label={`${c.name}`} value={c.id} />
          ))}
        </Picker>
      </View>
    </>
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Currency Pair</Text>

      <View style={globalStyles.card}>
        {renderCurrencyPicker(
          "Base Currency",
          baseCurrencyId,
          setBaseCurrencyId,
        )}

        {renderCurrencyPicker(
          "Quote Currency",
          quoteCurrencyId,
          setQuoteCurrencyId,
        )}

        <Text style={globalStyles.label}>Symbol</Text>
        <TextInput
          style={globalStyles.input}
          value={symbol}
          onChangeText={setSymbol}
          placeholder="e.g. EUR/USD"
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

export default UpdateCurrencyPairScreen;
