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

const AddFinancialReportScreen = () => {
  const router = useRouter();
  const { instrumentId } = useLocalSearchParams();
  const { width } = useWindowDimensions();

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const columnWidth = `${100 / columns - 4}%`;

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [instrument, setInstrument] = useState<any>(null);

  const [period, setPeriod] = useState("");
  const [revenue, setRevenue] = useState("");
  const [netIncome, setNetIncome] = useState("");
  const [eps, setEps] = useState("");
  const [assets, setAssets] = useState("");
  const [liabilities, setLiabilities] = useState("");
  const [operatingCashFlow, setOperatingCashFlow] = useState("");
  const [freeCashFlow, setFreeCashFlow] = useState("");

  useEffect(() => {
    const loadInstrument = async () => {
      try {
        const data = await ApiService.getInvestInstrumentById(
          Number(instrumentId),
        );
        setInstrument(data);
      } catch {
        Alert.alert("Error", "Failed to load investment instrument.");
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
      await ApiService.createFinancialReport({
        investInstrumentId: Number(instrumentId),
        period,
        revenue: Number(revenue),
        netIncome: Number(netIncome),
        eps: Number(eps),
        assets: Number(assets),
        liabilities: Number(liabilities),
        operatingCashFlow: Number(operatingCashFlow),
        freeCashFlow: Number(freeCashFlow),
      });

      Alert.alert("Success", "Financial report created.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create financial report.");
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const fields = [
    ["Period", period, setPeriod, "default"],
    ["Revenue", revenue, setRevenue, "numeric"],
    ["Net Income", netIncome, setNetIncome, "numeric"],
    ["EPS", eps, setEps, "numeric"],
    ["Assets", assets, setAssets, "numeric"],
    ["Liabilities", liabilities, setLiabilities, "numeric"],
    ["Operating Cash Flow", operatingCashFlow, setOperatingCashFlow, "numeric"],
    ["Free Cash Flow", freeCashFlow, setFreeCashFlow, "numeric"],
  ];

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Add Financial Report
      </Text>

      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <Text style={[globalStyles.cardTitle, spacing.mb1]}>
          {instrument?.name}
        </Text>
        <Text style={globalStyles.text}>Ticker: {instrument?.ticker}</Text>
      </View>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {fields.map(([label, value, setter, type], i) => (
          <View key={i} style={[spacing.m2, { width: columnWidth }]}>
            <View style={globalStyles.card}>
              <Text style={globalStyles.label}>{label}</Text>
              <TextInput
                style={globalStyles.input}
                value={value as string}
                onChangeText={setter as any}
                keyboardType={type === "numeric" ? "numeric" : "default"}
              />
            </View>
          </View>
        ))}
      </View>

      <View style={[spacing.mt4, globalStyles.fullWidth]}>
        <TouchableOpacity
          style={[globalStyles.button, saving && globalStyles.buttonDisabled]}
          disabled={saving}
          onPress={handleCreate}
        >
          {saving ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Create Report</Text>
          )}
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default AddFinancialReportScreen;
