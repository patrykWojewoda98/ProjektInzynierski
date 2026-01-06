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
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";

const AddFinancialMetricScreen = () => {
  const router = useRouter();
  const { instrumentId } = useLocalSearchParams();

  const [instrument, setInstrument] = useState(null);

  const [pe, setPe] = useState("");
  const [pb, setPb] = useState("");
  const [roe, setRoe] = useState("");
  const [debtToEquity, setDebtToEquity] = useState("");
  const [dividendYield, setDividendYield] = useState("");

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  // 📥 LOAD INSTRUMENT
  useEffect(() => {
    const loadInstrument = async () => {
      try {
        const data = await ApiService.getInvestInstrumentById(
          Number(instrumentId)
        );
        setInstrument(data);
      } catch {
        Alert.alert("Error", "Failed to load instrument.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    loadInstrument();
  }, [instrumentId]);

  const handleCreate = async () => {
    setSaving(true);

    try {
      await ApiService.createFinancialMetricForInstrument(
        Number(instrumentId),
        {
          pe: Number(pe),
          pb: Number(pb),
          roe: Number(roe),
          debtToEquity: Number(debtToEquity),
          dividendYield: Number(dividendYield),
        }
      );

      Alert.alert("Success", "Financial metrics created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create financial metrics.");
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Add Financial Metrics
      </Text>

      {/* 🔹 INSTRUMENT INFO */}
      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <Text style={[globalStyles.cardTitle, spacing.mb1]}>
          {instrument?.name}
        </Text>
        <Text style={globalStyles.text}>Ticker: {instrument?.ticker}</Text>
      </View>

      {/* 🔹 FORM */}
      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {[
          ["P/E", pe, setPe],
          ["P/B", pb, setPb],
          ["ROE (%)", roe, setRoe],
          ["Debt to Equity", debtToEquity, setDebtToEquity],
          ["Dividend Yield (%)", dividendYield, setDividendYield],
        ].map(([label, value, setter], i) => (
          <View key={i}>
            <Text style={globalStyles.label}>{label}</Text>
            <TextInput
              style={[globalStyles.input, spacing.mb3]}
              value={value}
              onChangeText={setter}
              keyboardType="numeric"
            />
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
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Create Metrics</Text>
        )}
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AddFinancialMetricScreen;
