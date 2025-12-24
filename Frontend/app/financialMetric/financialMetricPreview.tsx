import { useRouter } from "expo-router";
import React from "react";
import { Image, ScrollView, Text, TouchableOpacity, View } from "react-native";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";

const metrics = [
  {
    key: "pe",
    title: "P/E (Price to Earnings)",
    description:
      "Shows how much investors are willing to pay for one unit of company earnings.",
  },
  {
    key: "pb",
    title: "P/B (Price to Book)",
    description: "Compares the market value of a company to its book value.",
  },
  {
    key: "roe",
    title: "ROE (Return on Equity)",
    description:
      "Measures how efficiently a company generates profit from shareholders’ equity.",
  },
  {
    key: "debtToEquity",
    title: "Debt to Equity",
    description:
      "Indicates how much debt a company uses compared to its equity.",
  },
  {
    key: "dividendYield",
    title: "Dividend Yield",
    description:
      "Shows how much a company pays in dividends relative to its share price.",
  },
];

const FinancialMetricPreview = () => {
  const router = useRouter();

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, spacing.mb4]}
      />

      <Text style={[globalStyles.header, spacing.mb4]}>
        Financial Metrics Overview
      </Text>
      <Text style={[globalStyles.text, spacing.mb1]}>
        Zastanowić sie czy nie zaciągać tego z bazy
      </Text>

      <View style={globalStyles.fullWidth}>
        {metrics.map((m) => (
          <View key={m.key} style={[globalStyles.card, spacing.mb3]}>
            <Text style={globalStyles.cardTitle}>{m.title}</Text>
            <Text style={globalStyles.textSmall}>{m.description}</Text>
          </View>
        ))}
      </View>

      <TouchableOpacity
        style={spacing.mt6}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default FinancialMetricPreview;
