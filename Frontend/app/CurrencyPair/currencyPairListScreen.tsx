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

import { confirmAction } from "@/utils/confirmAction";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const CurrencyPairListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns();

  const [items, setItems] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getCurrencyPairs();
        setItems(data);
      } catch {
        Alert.alert("Error", "Failed to load currency pairs.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this currency pair?",
      onConfirm: async () => {
        try {
          await ApiService.deleteCurrencyPair(id);
          const refreshed = await ApiService.getCurrencyPairs();
          setItems(refreshed);
        } catch {
          Alert.alert("Error", "Failed to delete currency pair.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Currency Pairs</Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_CURRENCY_PAIR)}
      >
        <Text style={globalStyles.buttonText}>Add currency pair</Text>
      </TouchableOpacity>

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
            <Text style={globalStyles.cardTitle}>{i.symbol}</Text>

            <Text style={globalStyles.textSmall}>
              Base currency ID: {i.baseCurrencyId}
            </Text>
            <Text style={globalStyles.textSmall}>
              Quote currency ID: {i.quoteCurrencyId}
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
                    pathname: ROUTES.EDIT_CURRENCY_PAIR,
                    params: { id: i.id },
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

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default CurrencyPairListScreen;
