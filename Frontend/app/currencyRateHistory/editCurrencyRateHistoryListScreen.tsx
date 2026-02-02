import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { confirmAction } from "../../utils/confirmAction";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

type PairWithLatestRate = {
  pairId: number;
  symbol: string;
  latestRateId: number | null;
  rate: number | null;
  date: string | null;
};

const EditCurrencyRateHistoryListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns();

  const [items, setItems] = useState<PairWithLatestRate[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const pairs = await ApiService.getCurrencyPairs();
        const result: PairWithLatestRate[] = [];

        for (const p of pairs) {
          try {
            const latest = await ApiService.getLatestCurrencyRate(p.id);

            result.push({
              pairId: p.id,
              symbol: p.symbol,
              latestRateId: latest?.id ?? null,
              rate: latest?.closeRate ?? null,
              date: latest?.date ?? null,
            });
          } catch {
            result.push({
              pairId: p.id,
              symbol: p.symbol,
              latestRateId: null,
              rate: null,
              date: null,
            });
          }
        }

        setItems(result);
      } catch {
        Alert.alert("Error", "Failed to load currency rates.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleDelete = (rateId: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete latest currency rate?",
      onConfirm: async () => {
        try {
          await ApiService.deleteCurrencyRateHistory(rateId);
          setItems((prev) =>
            prev.map((x) =>
              x.latestRateId === rateId
                ? { ...x, latestRateId: null, rate: null, date: null }
                : x,
            ),
          );
        } catch {
          Alert.alert("Error", "Failed to delete currency rate.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Currency Rate History
      </Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {items.map((i) => (
          <View
            key={i.pairId}
            style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
          >
            <Text style={globalStyles.cardTitle}>{i.symbol}</Text>

            {i.rate !== null ? (
              <>
                <Text style={globalStyles.textSmall}>
                  Latest rate:{" "}
                  <Text style={{ color: COLORS.primary, fontWeight: "600" }}>
                    {i.rate}
                  </Text>
                </Text>

                <Text style={globalStyles.textSmall}>
                  Date: {new Date(i.date!).toLocaleDateString()}
                </Text>
              </>
            ) : (
              <Text style={globalStyles.textSmall}>No rate available</Text>
            )}

            <View
              style={[
                globalStyles.row,
                spacing.mt3,
                { justifyContent: "center" },
              ]}
            >
              {i.latestRateId === null && (
                <TouchableOpacity
                  onPress={() =>
                    router.push({
                      pathname: ROUTES.ADD_CURRENCY_RATE_HISTORY,
                      params: { pairId: String(i.pairId) },
                    })
                  }
                >
                  <Ionicons
                    name="add-circle"
                    size={28}
                    color={COLORS.success}
                  />
                </TouchableOpacity>
              )}

              {i.latestRateId !== null && (
                <>
                  <TouchableOpacity
                    style={spacing.mr4}
                    onPress={() =>
                      router.push({
                        pathname: ROUTES.EDIT_CURRENCY_RATE_HISTORY_LIST,
                        params: {
                          currencyPairId: String(i.pairId),
                          symbol: i.symbol,
                        },
                      })
                    }
                  >
                    <Ionicons name="pencil" size={22} color={COLORS.primary} />
                  </TouchableOpacity>

                  <TouchableOpacity
                    onPress={() => handleDelete(i.latestRateId!)}
                  >
                    <Ionicons name="trash" size={22} color={COLORS.error} />
                  </TouchableOpacity>
                </>
              )}
            </View>
          </View>
        ))}
      </View>

      {items.length === 0 && (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <Text style={[globalStyles.text, { textAlign: "center" }]}>
            No currency rates found.
          </Text>
        </View>
      )}

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default EditCurrencyRateHistoryListScreen;
