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

const EditFinancialReportScreen = () => {
  const router = useRouter();
  const { id } = useLocalSearchParams(); // reportId

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const [report, setReport] = useState(null);
  const [instrument, setInstrument] = useState(null);

  const [period, setPeriod] = useState("");
  const [revenue, setRevenue] = useState("");
  const [netIncome, setNetIncome] = useState("");
  const [eps, setEps] = useState("");
  const [assets, setAssets] = useState("");
  const [liabilities, setLiabilities] = useState("");
  const [operatingCashFlow, setOperatingCashFlow] = useState("");
  const [freeCashFlow, setFreeCashFlow] = useState("");

  // 📥 LOAD REPORT + INSTRUMENT
  useEffect(() => {
    const loadData = async () => {
      try {
        const reportData = await ApiService.getFinancialReportById(Number(id));
        setReport(reportData);

        setPeriod(reportData.period ?? "");
        setRevenue(String(reportData.revenue ?? ""));
        setNetIncome(String(reportData.netIncome ?? ""));
        setEps(String(reportData.eps ?? ""));
        setAssets(String(reportData.assets ?? ""));
        setLiabilities(String(reportData.liabilities ?? ""));
        setOperatingCashFlow(String(reportData.operatingCashFlow ?? ""));
        setFreeCashFlow(String(reportData.freeCashFlow ?? ""));

        const instrumentData = await ApiService.getInvestInstrumentById(
          reportData.investInstrumentId
        );
        setInstrument(instrumentData);
      } catch {
        Alert.alert("Error", "Failed to load financial report.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [id]);

  const handleSave = async () => {
    setSaving(true);

    try {
      await ApiService.updateFinancialReport(Number(id), {
        id: Number(id),
        investInstrumentId: instrument?.id,
        period,
        revenue: Number(revenue),
        netIncome: Number(netIncome),
        eps: Number(eps),
        assets: Number(assets),
        liabilities: Number(liabilities),
        operatingCashFlow: Number(operatingCashFlow),
        freeCashFlow: Number(freeCashFlow),
      });

      Alert.alert("Success", "Financial report updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update financial report.");
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
        Edit Financial Report
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
          ["Period", period, setPeriod],
          ["Revenue", revenue, setRevenue],
          ["Net Income", netIncome, setNetIncome],
          ["EPS", eps, setEps],
          ["Assets", assets, setAssets],
          ["Liabilities", liabilities, setLiabilities],
          ["Operating Cash Flow", operatingCashFlow, setOperatingCashFlow],
          ["Free Cash Flow", freeCashFlow, setFreeCashFlow],
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

export default EditFinancialReportScreen;
