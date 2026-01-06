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

const EditFinancialMetricScreen = () => {
  const router = useRouter();
  const { metricId } = useLocalSearchParams();

  const [pe, setPe] = useState("");
  const [pb, setPb] = useState("");
  const [roe, setRoe] = useState("");
  const [debtToEquity, setDebtToEquity] = useState("");
  const [dividendYield, setDividendYield] = useState("");

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    const load = async () => {
      try {
        const metric = await ApiService.getFinancialMetricById(
          Number(metricId)
        );

        setPe(String(metric.pe ?? ""));
        setPb(String(metric.pb ?? ""));
        setRoe(String(metric.roe ?? ""));
        setDebtToEquity(String(metric.debtToEquity ?? ""));
        setDividendYield(String(metric.dividendYield ?? ""));
      } catch {
        Alert.alert("Error", "Failed to load financial metrics.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [metricId]);

  const handleSave = async () => {
    setSaving(true);

    try {
      await ApiService.updateFinancialMetric(Number(metricId), {
        pe: Number(pe),
        pb: Number(pb),
        roe: Number(roe),
        debtToEquity: Number(debtToEquity),
        dividendYield: Number(dividendYield),
      });

      Alert.alert("Success", "Financial metrics updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update financial metrics.");
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
        Edit Financial Metrics
      </Text>

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
        onPress={handleSave}
      >
        {saving ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Save Changes</Text>
        )}
      </TouchableOpacity>
    </ScrollView>
  );
};

export default EditFinancialMetricScreen;
