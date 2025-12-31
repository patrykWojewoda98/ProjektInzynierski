import { useRouter } from "expo-router";
import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { ROUTES } from "../../routes";
import { AuthContext } from "../_layout";

const WatchList = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);

  const [instruments, setInstruments] = useState([]);

  // ⭐ investInstrumentId -> watchListItemId
  const [watchListMap, setWatchListMap] = useState({});

  const [loading, setLoading] = useState(true);
  const [watchListLoaded, setWatchListLoaded] = useState(false);

  // ⭐ TOGGLE WATCHLIST
  const handleToggleWatchList = async (instrumentId) => {
    if (!user?.id) return;

    const key = Number(instrumentId);
    const watchListItemId = watchListMap[key];

    try {
      if (watchListItemId) {
        await ApiService.deleteWatchListItem(watchListItemId);

        setWatchListMap((prev) => {
          const copy = { ...prev };
          delete copy[key];
          return copy;
        });

        setInstruments((prev) => prev.filter((x) => x.id !== key));
      }
    } catch (err) {
      console.error("Watchlist toggle error:", err);
    }
  };

  // ⭐ LOAD WATCHLIST + INSTRUMENTS
  useEffect(() => {
    const loadData = async () => {
      if (!user?.id) {
        setWatchListLoaded(true);
        setLoading(false);
        return;
      }

      try {
        // 1️⃣ WatchList items
        const raw = await ApiService.getWatchListItemsByClientId(user.id);
        const arr = Array.isArray(raw) ? raw : [raw];

        const map = {};
        arr.forEach((x) => {
          map[Number(x.investInstrumentId)] = x.id;
        });
        setWatchListMap(map);

        // 2️⃣ Wszystkie instrumenty
        const allInstruments = await ApiService.getInvestInstruments();

        // 3️⃣ TYLKO polubione
        const favorites = allInstruments.filter((i) => map[Number(i.id)]);

        setInstruments(favorites);
      } catch (err) {
        console.error("Error loading watchlist:", err);
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

      <View style={[spacing.mt3, globalStyles.fullWidth]}>
        {instruments.map((i) => (
          <View
            key={i.id}
            style={[globalStyles.card, { position: "relative" }]}
          >
            <TouchableOpacity
              style={globalStyles.star}
              onPress={() => handleToggleWatchList(i.id)}
            >
              <Ionicons
                name={watchListMap[i.id] ? "star" : "star-outline"}
                size={26}
                color="white"
              />
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
                spacing.mt1,
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
