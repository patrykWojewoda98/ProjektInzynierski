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
  useWindowDimensions,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const EditFinancialMetricScreen = () => {
  const router = useRouter();
  const { metricId } = useLocalSearchParams();
  const { width } = useWindowDimensions();

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const columnWidth = `${100 / columns - 4}%`;

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
          Number(metricId),
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

  const fields = [
    ["P/E", pe, setPe],
    ["P/B", pb, setPb],
    ["ROE (%)", roe, setRoe],
    ["Debt to Equity", debtToEquity, setDebtToEquity],
    ["Dividend Yield (%)", dividendYield, setDividendYield],
  ];

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Edit Financial Metrics
      </Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {fields.map(([label, value, setter], i) => (
          <View key={i} style={[spacing.m2, { width: columnWidth }]}>
            <View style={globalStyles.card}>
              <Text style={globalStyles.label}>{label}</Text>
              <TextInput
                style={globalStyles.input}
                value={value as string}
                onChangeText={setter as any}
                keyboardType="numeric"
              />
            </View>
          </View>
        ))}
      </View>

      <View style={[spacing.mt4, globalStyles.fullWidth]}>
        <TouchableOpacity
          style={[globalStyles.button, saving && globalStyles.buttonDisabled]}
          disabled={saving}
          onPress={handleSave}
        >
          {saving ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Save Changes</Text>
          )}
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default EditFinancialMetricScreen;
