import { Ionicons } from "@expo/vector-icons";
import { useLocalSearchParams, useRouter } from "expo-router";
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

type CurrencyRateHistoryItem = {
  id: number;
  date: string;
  openRate: number | null;
  highRate: number | null;
  lowRate: number | null;
  closeRate: number;
};

const EditCurrencyRateHistoryByPairScreen = () => {
  const { currencyPairId } = useLocalSearchParams();
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns();

  const pairId = Number(
    Array.isArray(currencyPairId) ? currencyPairId[0] : currencyPairId,
  );

  const [items, setItems] = useState<CurrencyRateHistoryItem[]>([]);
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
    if (!isReady || !pairId) return;

    const load = async () => {
      try {
        const data = await ApiService.getCurrencyRateHistoryByPair(pairId);
        setItems(
          data.sort(
            (a: CurrencyRateHistoryItem, b: CurrencyRateHistoryItem) =>
              new Date(b.date).getTime() - new Date(a.date).getTime(),
          ),
        );
      } catch {
        Alert.alert("Error", "Failed to load currency rate history.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady, pairId]);

  const handleDelete = (rateId: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this currency rate?",
      onConfirm: async () => {
        try {
          await ApiService.deleteCurrencyRateHistory(rateId);
          setItems((prev) => prev.filter((x) => x.id !== rateId));
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
        style={[globalStyles.row, spacing.mb4, { justifyContent: "center" }]}
      >
        <TouchableOpacity
          onPress={() =>
            router.push({
              pathname: ROUTES.ADD_CURRENCY_RATE_HISTORY,
              params: { currencyPairId: String(pairId) },
            })
          }
          style={[
            globalStyles.button,
            { flexDirection: "row", alignItems: "center" },
          ]}
        >
          <Ionicons
            name="add-circle-outline"
            size={20}
            color={COLORS.white}
            style={spacing.mr2}
          />
          <Text style={globalStyles.buttonText}>Add rate</Text>
        </TouchableOpacity>
      </View>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {items.map((i) => (
          <View
            key={i.id}
            style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
          >
            <Text style={globalStyles.cardTitle}>
              {new Date(i.date).toLocaleDateString()}
            </Text>

            <Text style={globalStyles.textSmall}>
              Open: {i.openRate ?? "-"}
            </Text>
            <Text style={globalStyles.textSmall}>
              High: {i.highRate ?? "-"}
            </Text>
            <Text style={globalStyles.textSmall}>Low: {i.lowRate ?? "-"}</Text>
            <Text style={globalStyles.textSmall}>
              Close:{" "}
              <Text style={{ color: COLORS.primary, fontWeight: "600" }}>
                {i.closeRate}
              </Text>
            </Text>

            <View
              style={[
                globalStyles.row,
                spacing.mt3,
                { justifyContent: "center" },
              ]}
            >
              <TouchableOpacity
                style={spacing.mr4}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_SPECIFIC_CURRENCY_RATE_HISTORY,
                    params: { rateId: String(i.id) },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDelete(i.id)}>
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
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

export default EditCurrencyRateHistoryByPairScreen;
