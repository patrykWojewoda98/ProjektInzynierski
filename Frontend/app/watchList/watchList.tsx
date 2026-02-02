import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";
import { AuthContext } from "../_layout";

const WatchList = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);
  const { itemWidth } = useResponsiveColumns();

  const [instruments, setInstruments] = useState<any[]>([]);
  const [watchListMap, setWatchListMap] = useState<Record<number, number>>({});
  const [loading, setLoading] = useState(true);
  const [watchListLoaded, setWatchListLoaded] = useState(false);

  const handleToggleWatchList = async (instrumentId: number) => {
    if (!user?.id) return;

    const watchListItemId = watchListMap[instrumentId];
    if (!watchListItemId) return;

    try {
      await ApiService.deleteWatchListItem(watchListItemId);

      setWatchListMap((prev) => {
        const copy = { ...prev };
        delete copy[instrumentId];
        return copy;
      });

      setInstruments((prev) => prev.filter((x) => x.id !== instrumentId));
    } catch (err) {
      console.error("Watchlist toggle error:", err);
    }
  };

  useEffect(() => {
    const loadData = async () => {
      if (!user?.id) {
        setWatchListLoaded(true);
        setLoading(false);
        return;
      }

      try {
        const raw = await ApiService.getWatchListItemsByClientId(user.id);
        const arr = Array.isArray(raw) ? raw : [raw];

        const map: Record<number, number> = {};
        arr.forEach((x) => {
          map[Number(x.investInstrumentId)] = x.id;
        });
        setWatchListMap(map);

        const allInstruments = await ApiService.getInvestInstruments();
        const favorites = allInstruments.filter((i) => map[i.id]);

        setInstruments(favorites);
      } catch {
        setInstruments([]);
        setWatchListMap({});
      } finally {
        setWatchListLoaded(true);
        setLoading(false);
      }
    };

    loadData();
  }, [user]);

  if (loading || !watchListLoaded) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>My Watchlist</Text>

      {instruments.length === 0 && (
        <Text style={[globalStyles.text, spacing.mt4]}>
          You don’t have any instruments in your watchlist yet.
        </Text>
      )}

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {instruments.map((i) => (
          <View
            key={i.id}
            style={[
              globalStyles.card,
              spacing.m2,
              { width: itemWidth, position: "relative" },
            ]}
          >
            <TouchableOpacity
              style={globalStyles.star}
              onPress={() => handleToggleWatchList(i.id)}
            >
              <Ionicons name="star" size={26} color={COLORS.whiteHeader} />
            </TouchableOpacity>

            <Text style={globalStyles.cardTitle}>Ticker: {i.ticker}</Text>
            <Text style={globalStyles.text}>Name: {i.name}</Text>
            <Text style={globalStyles.textSmall}>
              Description: {i.description}
            </Text>

            <TouchableOpacity
              style={[
                globalStyles.button,
                globalStyles.fullWidth,
                spacing.py2,
                spacing.mt2,
              ]}
              onPress={() =>
                router.push({
                  pathname: ROUTES.FINANCIAL_METRIC,
                  params: { id: i.id },
                })
              }
            >
              <Text style={globalStyles.buttonText}>See more</Text>
            </TouchableOpacity>
          </View>
        ))}
      </View>

      <TouchableOpacity
        style={[spacing.mt6]}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default WatchList;
