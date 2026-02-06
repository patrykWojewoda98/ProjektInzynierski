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

const screenWidth = Dimensions.get("window").width;

const WalletChartScreen: React.FC = () => {
  const { user } = useContext(AuthContext);

  const [data, setData] = useState<ChartItem[]>([]);
  const [totalValue, setTotalValue] = useState<number>(0);
  const [currency, setCurrency] = useState<string>("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadData = async () => {
      try {
        if (!user?.id) return;

        // 1️⃣ Wallet (żeby dostać walletId)
        const wallet = await ApiService.getWalletByClientId(user.id);

        // 2️⃣ Gotowa wycena z backendu
        const summary = await ApiService.getWalletInvestmentSummary(wallet.id);

        const chartData: ChartItem[] = [];
        let colorIndex = 0;

        // INVESTMENTS
        for (const item of summary.investments) {
          chartData.push({
            name: item.instrumentName,
            value: item.valueInAccountCurrency,
            color: COLORS.chartColors[colorIndex % COLORS.chartColors.length],
            legendFontColor: COLORS.textGrey,
            legendFontSize: 12,
          });

          colorIndex++;
        }

        // CASH
        chartData.push({
          name: "Free Cash",
          value: summary.cashBalance,
          color: COLORS.chartColors[colorIndex % COLORS.chartColors.length],
          legendFontColor: COLORS.textGrey,
          legendFontSize: 12,
        });

        setCurrency(summary.accountCurrency);
        setTotalValue(summary.totalAccountValue);
        setData(chartData);
      } catch (err) {
        console.error("Chart load error:", err);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [user]);

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
          {totalValue.toFixed(2)} {currency}
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
