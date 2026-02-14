import { useRouter } from "expo-router";
import React, { useCallback, useEffect, useState } from "react";
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
import ApiService from "../services/api";
import { getApiOrigin } from "../services/apiClient";
import { authGuard } from "../utils/authGuard";

const KEY_TO_ROUTE: Record<string, string> = {
  InvestProfile: ROUTES.INVEST_PROFILE,
  WatchList: ROUTES.WATCHLIST,
  Wallet: ROUTES.WALLET,
  MarketData: ROUTES.MARKET_DATA,
  InvestInstrument: ROUTES.INVEST_INSTRUMENT,
  FinancialReport: ROUTES.FINANCIAL_REPORT,
  FinancialMetric: ROUTES.FINANCIAL_METRIC_PREVIEW,
  MyAIAnalysisRequests: ROUTES.MY_AI_ANALYSIS_REQUESTS,
  CurrencyRateHistory: ROUTES.CURRENCY_RATE_HISTORY,
};

const fallbackIcons: Record<string, number> = {
  InvestProfile: require("../assets/images/Client-Icon.png"),
  WatchList: require("../assets/images/WatchList-Icon.png"),
  Wallet: require("../assets/images/Wallet-Icon.png"),
  MarketData: require("../assets/images/MarketData-Icon.png"),
  InvestInstrument: require("../assets/images/InvestInstrument-Icon.png"),
  FinancialReport: require("../assets/images/FinancialReport-Icon.png"),
  FinancialMetric: require("../assets/images/FinancialMetric-Icon.png"),
  MyAIAnalysisRequests: require("../assets/images/MyAIAnalysisRequests-Icon.png"),
  CurrencyRateHistory: require("../assets/images/CurrencyRateHistoryIcon.png"),
};

const defaultTiles = [
  {
    key: "InvestProfile",
    label: "My Investment Profile",
    route: ROUTES.INVEST_PROFILE,
  },
  { key: "WatchList", label: "Watchlist", route: ROUTES.WATCHLIST },
  { key: "Wallet", label: "Wallet", route: ROUTES.WALLET },
  { key: "MarketData", label: "Market Data", route: ROUTES.MARKET_DATA },
  {
    key: "InvestInstrument",
    label: "Investment Instruments",
    route: ROUTES.INVEST_INSTRUMENT,
  },
  {
    key: "FinancialReport",
    label: "Financial Reports",
    route: ROUTES.FINANCIAL_REPORT,
  },
  {
    key: "FinancialMetric",
    label: "Financial Metrics",
    route: ROUTES.FINANCIAL_METRIC_PREVIEW,
  },
  {
    key: "MyAIAnalysisRequests",
    label: "My AI Analysis Requests",
    route: ROUTES.MY_AI_ANALYSIS_REQUESTS,
  },
  {
    key: "CurrencyRateHistory",
    label: "Currency Rate History",
    route: ROUTES.CURRENCY_RATE_HISTORY,
  },
];

const PLATFORM = "Web";

const MainMenu = () => {
  const router = useRouter();
  const { width } = useWindowDimensions();
  const [isReady, setIsReady] = useState(false);
  const [menuItems, setMenuItems] = useState<
    { key: string; label: string; route: string; imagePath?: string; description?: string }[]
  >([]);

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const loadMenu = useCallback(async () => {
    try {
      const items = await ApiService.getClientConfigMenu(PLATFORM);
      if (Array.isArray(items) && items.length > 0) {
        const mapped = items
          .map(
            (item: {
              key: string;
              displayText: string;
              description?: string;
              imagePath?: string;
              orderIndex: number;
            }) => {
              const route = KEY_TO_ROUTE[item.key];
              if (!route) return null;
              return {
                key: item.key,
                label: item.displayText,
                route,
                imagePath: item.imagePath,
                description: item.description,
              };
            },
          )
          .filter(Boolean) as {
          key: string;
          label: string;
          route: string;
          imagePath?: string;
          description?: string;
        }[];
        setMenuItems(mapped);
      } else {
        setMenuItems(defaultTiles.map((t) => ({ ...t, imagePath: undefined, description: undefined })));
      }
    } catch {
      setMenuItems(defaultTiles.map((t) => ({ ...t, imagePath: undefined, description: undefined })));
    }
  }, []);

  useEffect(() => {
    const checkAuth = async () => {
      const isValid = await authGuard();
      if (isValid) {
        setIsReady(true);
        loadMenu();
      }
    };
    checkAuth();
  }, [loadMenu]);

  if (!isReady) {
    return (
      <View style={globalStyles.centerContainer}>
        <Text style={globalStyles.header}>Checking authentication...</Text>
      </View>
    );
  }

  const columns = getColumns();
  const tileWidth = `${100 / columns - 4}%`;
  const API_BASE = getApiOrigin();

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
          resizeMode="contain"
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
        {menuItems.map((t) => (
          <TouchableOpacity
            key={t.key}
            onPress={() => router.push(t.route)}
            style={[
              globalStyles.menuTile,
              spacing.m2,
              { width: tileWidth, minHeight: 170 },
            ]}
          >
            {t.imagePath ? (
              <Image
                source={{
                  uri: t.imagePath.startsWith("http")
                    ? t.imagePath
                    : `${API_BASE}/${t.imagePath}`,
                }}
                style={globalStyles.menuIcon}
                resizeMode="contain"
              />
            ) : (
              <Image
                source={fallbackIcons[t.key] ?? fallbackIcons.InvestProfile}
                style={globalStyles.menuIcon}
                resizeMode="contain"
              />
            )}
            <Text style={globalStyles.menuLabel}>{t.label}</Text>
            {t.description ? (
              <Text style={[globalStyles.textSmall, { textAlign: "center", marginTop: 4 }]}>
                {t.description}
              </Text>
            ) : null}
          </TouchableOpacity>
        ))}
      </View>
    </ScrollView>
  );
};

export default MainMenu;
