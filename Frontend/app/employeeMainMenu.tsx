import { useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
import { Image, ScrollView, Text, TouchableOpacity, View } from "react-native";
import { globalStyles, spacing } from "../assets/styles/styles";
import { authGuard } from "../utils/authGuard";
import { ROUTES } from "../routes";

const icons = {
  InvestProfile: require("../assets/images/Client-Icon.png"),
  Wallet: require("../assets/images/Wallet-Icon.png"),
  MarketData: require("../assets/images/MarketData-Icon.png"),
  InvestInstrument: require("../assets/images/InvestInstrument-Icon.png"),
  FinancialReport: require("../assets/images/FinancialReport-Icon.png"),
  FinancialMetric: require("../assets/images/FinancialMetric-Icon.png"),
};

const tiles = [
  {
    key: "InvestProfile",
    label: "Edit Investment Profile",
    icon: icons.InvestProfile,
    route: ROUTES.INVEST_PROFILE,
  },
  {
    key: "Wallet",
    label: "Edit Wallet",
    icon: icons.Wallet,
    route: ROUTES.WALLET,
  },

  {
    key: "MarketData",
    label: "Edit Market Data",
    icon: icons.MarketData,
    route: ROUTES.MARKET_DATA,
  },
  {
    key: "InvestInstrument",
    label: "Edit Investment Instruments",
    icon: icons.InvestInstrument,
    route: ROUTES.INVEST_INSTRUMENT,
  },
  {
    key: "FinancialReport",
    label: "Edit Financial Reports",
    icon: icons.FinancialReport,
    route: ROUTES.FINANCIAL_REPORT,
  },
  {
    key: "FinancialMetric",
    label: "Edit Financial Metrics",
    icon: icons.FinancialMetric,
    route: ROUTES.FINANCIAL_METRIC_PREVIEW,
  },
];

const EmployeeMainMenu = () => {
  const router = useRouter();
  const [isReady, setIsReady] = useState(false);

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
      <View style={[globalStyles.centerContainer]}>
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
      {/* Nagłówek */}
      <View style={[globalStyles.center, spacing.mb6]}>
        <Image
          source={require("../assets/images/Logo.png")}
          style={[globalStyles.logo, spacing.mb4]}
        />
        <Text style={[globalStyles.header, spacing.mb2]}>Main Menu</Text>
      </View>

      {/* Siatka kafelków */}
      <View style={globalStyles.menuGrid}>
        {tiles.map((t) => (
          <TouchableOpacity
            key={t.key}
            onPress={() => router.push(t.route)}
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

export default EmployeeMainMenu;
