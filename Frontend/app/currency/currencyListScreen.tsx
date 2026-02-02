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

const CurrencyListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(3);

  const [currencies, setCurrencies] = useState<any[]>([]);
  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    employeeAuthGuard().then(setIsReady);
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const [currencyData, riskData] = await Promise.all([
          ApiService.getAllCurrencies(),
          ApiService.getRiskLevels(),
        ]);

        setCurrencies(currencyData);
        setRiskLevels(riskData);
      } catch {
        Alert.alert("Error", "Failed to load currencies.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const getRiskDescription = (id: number) =>
    riskLevels.find((r) => r.id === id)?.description ?? "N/A";

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm action",
      message: "Are you sure you want to continue?",
      onConfirm: async () => {
        try {
          await ApiService.deleteCurrency(id);
          setCurrencies((p) => p.filter((c) => c.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete currency.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Currencies</Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_CURRENCY)}
      >
        <Text style={globalStyles.buttonText}>Add new Currency</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {currencies.map((currency) => (
          <View
            key={currency.id}
            style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
          >
            <Text style={globalStyles.cardTitle}>
              {currency.name} ({currency.code})
            </Text>

            <Text style={[globalStyles.textSmall, spacing.mb2]}>
              Risk level: {getRiskDescription(currency.currencyRiskLevelId)}
            </Text>

            <View
              style={[
                globalStyles.row,
                spacing.mt2,
                { justifyContent: "center" },
              ]}
            >
              <TouchableOpacity
                style={spacing.mr4}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_CURRENCY,
                    params: { id: currency.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDelete(currency.id)}>
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
            </View>
          </View>
        ))}
      </View>

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default CurrencyListScreen;
