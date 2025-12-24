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

const FinancialMetric = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [metric, setMetric] = useState<any>(null);
  const [instrument, setInstrument] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    const loadData = async () => {
      try {
        const instrumentData = await ApiService.getInvestInstrumentById(
          Number(id)
        );
        setInstrument(instrumentData);

        if (instrumentData?.financialMetricId) {
          const metricData = await ApiService.getFinancialMetricById(
            instrumentData.financialMetricId
          );
          setMetric(metricData);
        }
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [id]);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Metrics</Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* INSTRUMENT */}
        <Text style={[globalStyles.cardTitle, spacing.mb2]}>Instrument</Text>

        <Text style={globalStyles.text}>Name: {instrument?.name}</Text>
        <Text style={globalStyles.text}>Ticker: {instrument?.ticker}</Text>
        <Text style={globalStyles.text}>
          Description: {instrument?.description}
        </Text>
        <Text style={globalStyles.text}>
          Market Date: {instrument?.marketDataDate}
        </Text>
      </View>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* METRICS */}
        {metric && (
          <>
            <View style={spacing.mt3} />

            <Text style={[globalStyles.cardTitle, spacing.mb2]}>
              Financial Ratios
            </Text>

            <Text style={globalStyles.text}>
              P/E (Price to Earnings): {metric.pe}
            </Text>
            <Text style={globalStyles.text}>
              P/B (Price to Book): {metric.pb}
            </Text>
            <Text style={globalStyles.text}>
              ROE (Return on Equity): {metric.roe}%
            </Text>
            <Text style={globalStyles.text}>
              Debt to Equity: {metric.debtToEquity}
            </Text>
            <Text style={globalStyles.text}>
              Dividend Yield: {metric.dividendYield}%
            </Text>
          </>
        )}
      </View>
      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          globalStyles.buttonDisabled,
          spacing.py3,
        ]}
      >
        <Text style={globalStyles.buttonText}>AI Investment Insight</Text>
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

export default FinancialMetric;
