import { Picker } from "@react-native-picker/picker";
import React, { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Dimensions,
  ScrollView,
  Text,
  View,
} from "react-native";
import { LineChart } from "react-native-chart-kit";

import { authGuard } from "@/utils/authGuard";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const screenWidth = Dimensions.get("window").width;

const CurrencyRateHistoryScreen = () => {
  const [isReady, setIsReady] = useState(false);
  const [loading, setLoading] = useState(true);

  const [currencyPairs, setCurrencyPairs] = useState<any[]>([]);
  const [selectedPairId, setSelectedPairId] = useState<number>(0);

  const [rates, setRates] = useState<any[]>([]);

  useEffect(() => {
    const check = async () => {
      const ok = await authGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const loadPairs = async () => {
      try {
        const data = await ApiService.getCurrencyPairs();
        setCurrencyPairs(data);
      } catch {
        setCurrencyPairs([]);
      } finally {
        setLoading(false);
      }
    };

    loadPairs();
  }, [isReady]);

  useEffect(() => {
    if (!selectedPairId) return;

    const loadHistory = async () => {
      setLoading(true);
      try {
        const data =
          await ApiService.getCurrencyRateHistoryByPair(selectedPairId);

        const sorted = data.sort(
          (a: any, b: any) =>
            new Date(a.date).getTime() - new Date(b.date).getTime(),
        );

        setRates(sorted);
      } catch {
        setRates([]);
      } finally {
        setLoading(false);
      }
    };

    loadHistory();
  }, [selectedPairId]);

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const chartData = {
    labels: rates.map((r) => new Date(r.date).toLocaleDateString()),
    datasets: [
      {
        data: rates.map((r) => r.closeRate),
        strokeWidth: 2,
      },
    ],
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Currency Rate History
      </Text>

      <View style={[globalStyles.card, spacing.mb4]}>
        <Text style={globalStyles.label}>Select currency pair</Text>

        <View style={globalStyles.pickerWrapper}>
          <Picker
            selectedValue={selectedPairId}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
            onValueChange={(v) => setSelectedPairId(Number(v))}
          >
            <Picker.Item label="Select pair" value={0} />
            {currencyPairs.map((p) => (
              <Picker.Item key={p.id} label={p.symbol} value={p.id} />
            ))}
          </Picker>
        </View>
      </View>

      {rates.length > 0 ? (
        <View style={globalStyles.card}>
          <LineChart
            data={chartData}
            width={screenWidth - 40}
            height={260}
            chartConfig={{
              backgroundGradientFrom: COLORS.cardBackground,
              backgroundGradientTo: COLORS.cardBackground,
              decimalPlaces: 4,
              color: () => COLORS.primary,
              labelColor: () => COLORS.textGrey,
              propsForDots: {
                r: "3",
                strokeWidth: "1",
                stroke: COLORS.primary,
              },
            }}
            bezier
            style={{ borderRadius: 12 }}
          />
        </View>
      ) : selectedPairId ? (
        <View style={globalStyles.card}>
          <Text style={{ textAlign: "center", color: COLORS.textGrey }}>
            No data available for selected currency pair.
          </Text>
        </View>
      ) : null}
    </ScrollView>
  );
};

export default CurrencyRateHistoryScreen;
