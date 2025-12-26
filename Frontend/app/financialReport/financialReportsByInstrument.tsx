import { useLocalSearchParams, useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Image,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";

const FinancialReportsByInstrument = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [reports, setReports] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    const loadReports = async () => {
      try {
        const data = await ApiService.getFinancialReportsByInvestInstrumentId(
          Number(id)
        );
        setReports(data);
      } catch {
        setReports([]);
      } finally {
        setLoading(false);
      }
    };

    loadReports();
  }, [id]);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, spacing.mb4]}
      />

      <Text style={[globalStyles.header, spacing.mb4]}>Financial Reports</Text>

      {reports.length === 0 ? (
        <View style={globalStyles.card}>
          <Text style={globalStyles.textSmall}>
            No financial reports available for this instrument.
          </Text>
        </View>
      ) : (
        <View style={globalStyles.fullWidth}>
          {reports.map((r) => (
            <View key={r.id} style={globalStyles.card}>
              <Text style={globalStyles.cardTitle}>{r.period}</Text>

              <Text style={globalStyles.textSmall}>
                Revenue: {r.revenue ?? "N/A"}
              </Text>
              <Text style={globalStyles.textSmall}>
                Net Income: {r.netIncome ?? "N/A"}
              </Text>
              <Text style={globalStyles.textSmall}>EPS: {r.eps ?? "N/A"}</Text>
              <Text style={globalStyles.textSmall}>
                Assets: {r.assets ?? "N/A"}
              </Text>
              <Text style={globalStyles.textSmall}>
                Liabilities: {r.liabilities ?? "N/A"}
              </Text>
              <Text style={globalStyles.textSmall}>
                Operating Cash Flow: {r.operatingCashFlow ?? "N/A"}
              </Text>
              <Text style={globalStyles.textSmall}>
                Free Cash Flow: {r.freeCashFlow ?? "N/A"}
              </Text>
            </View>
          ))}
        </View>
      )}

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          globalStyles.buttonDisabled,
          spacing.py2,
          spacing.mt1,
        ]}
        onPress={() =>
          router.push({
            pathname: ROUTES.CREATE_FINANCIAL_REPORT_BY_AI,
          })
        }
      >
        <Text style={globalStyles.buttonText}>
          Create new Financial Report Data
        </Text>
      </TouchableOpacity>
      <TouchableOpacity
        style={[spacing.mt6]}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default FinancialReportsByInstrument;
