import { Picker } from "@react-native-picker/picker";
import React, { useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  View,
  useWindowDimensions,
} from "react-native";
import { LineChart } from "react-native-chart-kit";

import { authGuard } from "@/utils/authGuard";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const CurrencyRateHistoryScreen = () => {
  const { width } = useWindowDimensions();
  const { itemWidth } = useResponsiveColumns();

  const [isReady, setIsReady] = useState(false);
  const [loading, setLoading] = useState(true);

  const [currencyPairs, setCurrencyPairs] = useState<any[]>([]);
  const [selectedPairId, setSelectedPairId] = useState<number | "all">("all");
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
      } finally {
        setLoading(false);
      }
    };

    loadPairs();
  }, [isReady]);

  useEffect(() => {
    if (selectedPairId === "all") {
      setRates([]);
      return;
    }

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

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[spacing.m2, { width: itemWidth }]}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>Currency Pair</Text>
            <View
              style={[
                globalStyles.pickerWrapper,
                globalStyles.pickerWebWrapper,
                {
                  flexDirection: "row",
                  alignItems: "center",
                  paddingHorizontal: 12,
                  height: 48,
                },
              ]}
            >
              <Picker
                selectedValue={selectedPairId}
                onValueChange={(v) =>
                  setSelectedPairId(v === "all" ? "all" : Number(v))
                }
                style={[
                  globalStyles.pickerText,
                  globalStyles.pickerWeb,
                  { flex: 1 },
                ]}
                dropdownIconColor={COLORS.textGrey}
              >
                <Picker.Item label="Select pair" value="all" />
                {currencyPairs.map((p) => (
                  <Picker.Item key={p.id} label={p.symbol} value={p.id} />
                ))}
              </Picker>
            </View>
          </View>
        </View>
      </View>

      {rates.length > 0 ? (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <LineChart
            data={chartData}
            width={width - 40}
            height={260}
            chartConfig={{
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
      ) : selectedPairId !== "all" ? (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <Text style={{ textAlign: "center", color: COLORS.textGrey }}>
            No data available for selected currency pair.
          </Text>
        </View>
      ) : null}
    </ScrollView>
  );
};

export default CurrencyRateHistoryScreen;
