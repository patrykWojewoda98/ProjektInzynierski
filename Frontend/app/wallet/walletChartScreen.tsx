import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Dimensions,
  ScrollView,
  Text,
  View,
} from "react-native";
import { PieChart } from "react-native-chart-kit";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { AuthContext } from "../_layout";

type ChartItem = {
  name: string;
  value: number;
  color: string;
  legendFontColor: string;
  legendFontSize: number;
};

type MarketData = {
  date: string;
  closePrice: number;
};

const screenWidth = Dimensions.get("window").width;

const WalletChartScreen: React.FC = () => {
  const { user } = useContext(AuthContext);

  const [data, setData] = useState<ChartItem[]>([]);
  const [totalValue, setTotalValue] = useState<number>(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadData = async () => {
      try {
        const wallet = await ApiService.getWalletByClientId(user!.id);
        const summary = await ApiService.getWalletInvestmentSummary(wallet.id);

        const result: ChartItem[] = [];
        let investmentsValue = 0;
        let colorIndex = 0;

        for (const item of summary) {
          const marketData: MarketData[] =
            await ApiService.getMarketDataByInstrumentId(item.instrumentId);

          if (!marketData || marketData.length === 0) continue;

          const latest = marketData.reduce((a, b) =>
            new Date(a.date) > new Date(b.date) ? a : b,
          );

          const value = item.totalQuantity * latest.closePrice;
          investmentsValue += value;

          result.push({
            name: item.instrumentName,
            value,
            color: COLORS.chartColors[colorIndex % COLORS.chartColors.length],
            legendFontColor: COLORS.textGrey,
            legendFontSize: 12,
          });

          colorIndex++;
        }

        result.push({
          name: "Free Cash",
          value: wallet.cashBalance,
          color: COLORS.chartColors[colorIndex % COLORS.chartColors.length],
          legendFontColor: COLORS.textGrey,
          legendFontSize: 12,
        });

        setTotalValue(wallet.cashBalance + investmentsValue);
        setData(result);
      } catch (err) {
        console.error("Chart load error:", err);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, []);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb1]}>
        Portfolio Structure
      </Text>

      {/* TOTAL VALUE */}
      <Text style={[globalStyles.subtitle, spacing.mb4]}>
        Total value:{" "}
        <Text style={{ color: COLORS.primary, fontWeight: "600" }}>
          {totalValue.toFixed(2)}
        </Text>
      </Text>

      <View style={globalStyles.card}>
        <PieChart
          data={data}
          width={screenWidth - 40}
          height={260}
          accessor="value"
          backgroundColor="transparent"
          paddingLeft="15"
          absolute
          chartConfig={{
            color: () => COLORS.textGrey,
          }}
        />
      </View>
    </ScrollView>
  );
};

export default WalletChartScreen;
