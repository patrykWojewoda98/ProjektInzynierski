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
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const AddCurrencyPairScreen = () => {
  const router = useRouter();

  const [currencies, setCurrencies] = useState<any[]>([]);

  const [baseCurrencyId, setBaseCurrencyId] = useState<number>(0);
  const [quoteCurrencyId, setQuoteCurrencyId] = useState<number>(0);
  const [symbol, setSymbol] = useState("");

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  // 🔐 AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  // 📥 LOAD CURRENCIES
  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getAllCurrencies();
        setCurrencies(data);
      } catch {
        Alert.alert("Error", "Failed to load currencies.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  // 🧠 AUTO SYMBOL (opcjonalne, ale bardzo UX)
  useEffect(() => {
    if (baseCurrencyId > 0 && quoteCurrencyId > 0) {
      const base = currencies.find((c) => c.id === baseCurrencyId);
      const quote = currencies.find((c) => c.id === quoteCurrencyId);

      if (base && quote) {
        setSymbol(`${base.name}/${quote.name}`);
      }
    }
  }, [baseCurrencyId, quoteCurrencyId, currencies]);

  const handleSave = async () => {
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

    if (!symbol.trim()) {
      Alert.alert("Validation error", "Symbol is required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.createCurrencyPair({
        baseCurrencyId,
        quoteCurrencyId,
        symbol: symbol.trim(),
      });

      Alert.alert("Success", "Currency pair created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create currency pair.");
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
      <Text style={[globalStyles.header, spacing.mb4]}>Add Currency Pair</Text>

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
          <Text style={globalStyles.buttonText}>Create currency pair</Text>
        )}
      </TouchableOpacity>

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Changed your mind?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default AddCurrencyPairScreen;
