import { useRouter } from "expo-router";
import React from "react";
import { Image, ScrollView, Text, TouchableOpacity, View } from "react-native";
import { globalStyles, spacing } from "../assets/styles/styles";

const icons = {
  WatchList: require("../assets/images/WatchList-Icon.png"),
  Wallet: require("../assets/images/Wallet-Icon.png"),
  TradeHistory: require("../assets/images/TradeHistory-Icon.png"),
  MarketData: require("../assets/images/MarketData-Icon.png"),
  InvestInstrument: require("../assets/images/InvestInstrument-Icon.png"),
  FinancialReport: require("../assets/images/FinancialReport-Icon.png"),
  FinancialMetric: require("../assets/images/FinancialMetric-Icon.png"),
  Client: require("../assets/images/Client-Icon.png"),
};

const tiles = [
  { key: "WatchList", label: "Watchlist", icon: icons.WatchList },
  { key: "Wallet", label: "Wallet", icon: icons.Wallet },
  { key: "TradeHistory", label: "Trade History", icon: icons.TradeHistory },
  { key: "MarketData", label: "Market Data", icon: icons.MarketData },
  {
    key: "InvestInstrument",
    label: "Investment Instruments",
    icon: icons.InvestInstrument,
  },
  {
    key: "FinancialReport",
    label: "Financial Reports",
    icon: icons.FinancialReport,
  },
  {
    key: "FinancialMetric",
    label: "Financial Metrics",
    icon: icons.FinancialMetric,
  },
  { key: "Client", label: "Clients", icon: icons.Client },
];

const MainMenu = () => {
  const router = useRouter();

  return (
    <ScrollView
      contentContainerStyle={[
        globalStyles.scrollContainer,
        globalStyles.alignCenter,
      ]}
      keyboardShouldPersistTaps="handled"
    >
      {/* Nagłówek */}
      <View style={[globalStyles.center, spacing.mb6]}>
        <Image
          source={require("../assets/images/Logo.png")}
          style={[globalStyles.logo, spacing.mb4]}
        />
        <Text style={[globalStyles.header, spacing.mb2]}>Main Menu</Text>
        <Text style={[globalStyles.text, { textAlign: "center" }]}>
          Choose a section to explore
        </Text>
      </View>

      {/* Siatka kafelków */}
      <View style={globalStyles.menuGrid}>
        {tiles.map((t) => (
          <TouchableOpacity
            key={t.key}
            onPress={() => router.push("/ComingSoon" as any)}
            style={globalStyles.menuTile}
          >
            <Image source={t.icon} style={globalStyles.menuIcon} />
            <Text style={globalStyles.menuLabel}>{t.label}</Text>
          </TouchableOpacity>
        ))}
      </View>
    </ScrollView>
  );
};

export default MainMenu;
