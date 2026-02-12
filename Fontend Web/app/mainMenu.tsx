import { useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
import {
  Image,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
  useWindowDimensions,
} from "react-native";

import { globalStyles, spacing } from "../assets/styles/styles";
import { ROUTES } from "../routes";
import { authGuard } from "../utils/authGuard";

const icons = {
  InvestProfile: require("../assets/images/Client-Icon.png"),
  WatchList: require("../assets/images/WatchList-Icon.png"),
  Wallet: require("../assets/images/Wallet-Icon.png"),
  MarketData: require("../assets/images/MarketData-Icon.png"),
  InvestInstrument: require("../assets/images/InvestInstrument-Icon.png"),
  FinancialReport: require("../assets/images/FinancialReport-Icon.png"),
  FinancialMetric: require("../assets/images/FinancialMetric-Icon.png"),
  CurrencyRateHistory: require("../assets/images/CurrencyRateHistoryIcon.png"),
};

const tiles = [
  {
    key: "InvestProfile",
    label: "My Investment Profile",
    icon: icons.InvestProfile,
    route: ROUTES.INVEST_PROFILE,
  },
  {
    key: "WatchList",
    label: "Watchlist",
    icon: icons.WatchList,
    route: ROUTES.WATCHLIST,
  },
  {
    key: "Wallet",
    label: "Wallet",
    icon: icons.Wallet,
    route: ROUTES.WALLET,
  },
  {
    key: "MarketData",
    label: "Market Data",
    icon: icons.MarketData,
    route: ROUTES.MARKET_DATA,
  },
  {
    key: "InvestInstrument",
    label: "Investment Instruments",
    icon: icons.InvestInstrument,
    route: ROUTES.INVEST_INSTRUMENT,
  },
  {
    key: "FinancialReport",
    label: "Financial Reports",
    icon: icons.FinancialReport,
    route: ROUTES.FINANCIAL_REPORT,
  },
  {
    key: "FinancialMetric",
    label: "Financial Metrics",
    icon: icons.FinancialMetric,
    route: ROUTES.FINANCIAL_METRIC_PREVIEW,
  },
  {
    key: "MyAIAnalysisRequests",
    label: "My AI Analysis Requests",
    icon: require("../assets/images/MyAIAnalysisRequests-Icon.png"),
    route: ROUTES.MY_AI_ANALYSIS_REQUESTS,
  },
  {
    key: "CurrencyRateHistory",
    label: "Currency Rate History",
    icon: icons.CurrencyRateHistory,
    route: ROUTES.CURRENCY_RATE_HISTORY,
  },
];

const MainMenu = () => {
  const router = useRouter();
  const { width } = useWindowDimensions();
  const [isReady, setIsReady] = useState(false);

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const tileWidth = `${100 / columns - 4}%`;

  useEffect(() => {
    const checkAuth = async () => {
      const isValid = await authGuard();
      if (isValid) {
        setIsReady(true);
      }
    };

    checkAuth();
  }, []);

  if (!isReady) {
    return (
      <View style={globalStyles.centerContainer}>
        <Text style={globalStyles.header}>Checking authentication...</Text>
      </View>
    );
  }

  return (
    <ScrollView
      contentContainerStyle={[
        globalStyles.scrollContainer,
        globalStyles.alignCenter,
      ]}
      keyboardShouldPersistTaps="handled"
    >
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

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {tiles.map((t) => (
          <TouchableOpacity
            key={t.key}
            onPress={() => router.push(t.route)}
            style={[
              globalStyles.menuTile,
              spacing.m2,
              { width: tileWidth, minHeight: 170 },
            ]}
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
