import { useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
import { Image, ScrollView, Text, TouchableOpacity, View } from "react-native";
import { globalStyles, spacing } from "../assets/styles/styles";
import { employeeAuthGuard } from "../utils/employeeAuthGuard";
import { ROUTES } from "../routes";

const icons = {
  InvestProfile: require("../assets/images/Client-Icon.png"),
  Wallet: require("../assets/images/Wallet-Icon.png"),
  MarketData: require("../assets/images/MarketData-Icon.png"),
  InvestInstrument: require("../assets/images/InvestInstrument-Icon.png"),
  FinancialReport: require("../assets/images/FinancialReport-Icon.png"),
  FinancialMetric: require("../assets/images/FinancialMetric-Icon.png"),
  Sector: require("../assets/images/SectorIcon.png"),
  RiskLevel: require("../assets/images/RiskLevelIcon.png"),
  InvestmentType: require("../assets/images/InvestmentTypeIcon.png"),
  InvestHorizon: require("../assets/images/InvestHorizonIcon.png"),
  Employee: require("../assets/images/EmployeeIcon.png"),
  Country: require("../assets/images/CountryIcon.png"),
  Region: require("../assets/images/RegionIcon.png"),
  Currency: require("../assets/images/CurrencyIcon.png"),
  RegionCode: require("../assets/images/RegionCodeIcon.png"),
};

const tiles = [
  {
    key: "InvestProfile",
    label: "Edit Investment Profile",
    icon: icons.InvestProfile,
    route: ROUTES.EDIT_INVEST_PROFILE,
  },
  {
    key: "Wallet",
    label: "Edit Wallet",
    icon: icons.Wallet,
    route: ROUTES.EDIT_WALLET,
  },
  {
    key: "MarketData",
    label: "Edit Market Data",
    icon: icons.MarketData,
    route: ROUTES.EDIT_MARKET_DATA,
  },
  {
    key: "InvestInstrument",
    label: "Edit Investment Instruments",
    icon: icons.InvestInstrument,
    route: ROUTES.EDIT_INVEST_INSTRUMENT,
  },
  {
    key: "FinancialReport",
    label: "Edit Financial Reports",
    icon: icons.FinancialReport,
    route: ROUTES.EDIT_FINANCIAL_REPORT,
  },
  {
    key: "FinancialMetric",
    label: "Edit Financial Metrics",
    icon: icons.FinancialMetric,
    route: ROUTES.EDIT_FINANCIAL_METRIC_PREVIEW,
  },

  {
    key: "Sector",
    label: "Edit Sectors",
    icon: icons.Sector,
    route: ROUTES.EDIT_SECTOR,
  },
  {
    key: "RiskLevel",
    label: "Edit Risk Levels",
    icon: icons.RiskLevel,
    route: ROUTES.EDIT_RISK_LEVEL,
  },
  {
    key: "InvestmentType",
    label: "Edit Investment Types",
    icon: icons.InvestmentType,
    route: ROUTES.EDIT_INVESTMENT_TYPE,
  },
  {
    key: "InvestHorizon",
    label: "Edit Investment Horizons",
    icon: icons.InvestHorizon,
    route: ROUTES.EDIT_INVEST_HORIZON,
  },
  {
    key: "Employee",
    label: "Edit Employees",
    icon: icons.Employee,
    route: ROUTES.EDIT_EMPLOYEE,
  },
  {
    key: "Country",
    label: "Edit Countries",
    icon: icons.Country,
    route: ROUTES.COUNTRY,
  },
  {
    key: "Region",
    label: "Edit Regions",
    icon: icons.Region,
    route: ROUTES.REGION,
  },
  {
    key: "Currency",
    label: "Edit Currencies",
    icon: icons.Currency,
    route: ROUTES.CURRENCY,
  },
  {
    key: "RegionCode",
    label: "Edit Region Codes",
    icon: icons.Region,
    route: ROUTES.REGION_CODE,
  },
];

const EmployeeMainMenu = () => {
  const router = useRouter();
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      const isValid = await employeeAuthGuard();
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
