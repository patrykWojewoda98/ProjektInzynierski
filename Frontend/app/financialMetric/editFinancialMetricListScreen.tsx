import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useEffect, useMemo, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { confirmAction } from "../../utils/confirmAction";

const EditFinancialMetricListScreen = () => {
  const router = useRouter();

  const [instruments, setInstruments] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");
  const [importingId, setImportingId] = useState<number | null>(null);

  useEffect(() => {
    const load = async () => {
      try {
        const data = await ApiService.getInvestInstruments();
        setInstruments(data);
      } catch {
        Alert.alert("Error", "Failed to load investment instruments.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, []);

  const filteredInstruments = useMemo(() => {
    const q = search.toLowerCase();

    return instruments.filter(
      (i) =>
        i.name?.toLowerCase().includes(q) ||
        i.ticker?.toLowerCase().includes(q) ||
        i.isin?.toLowerCase().includes(q),
    );
  }, [search, instruments]);

  const handleAutoImport = async (instrumentId: number) => {
    try {
      setImportingId(instrumentId);

      await ApiService.importFinancialMetric({
        investInstrumentId: instrumentId,
      });

      const refreshed = await ApiService.getInvestInstruments();
      setInstruments(refreshed);
    } catch {
      Alert.alert(
        "Error",
        "Failed to automatically load or update financial metrics.",
      );
    } finally {
      setImportingId(null);
    }
  };

  const handleDeleteMetric = (metricId: number) => {
    confirmAction({
      title: "Confirm action",
      message: "Are you sure you want to delete this financial metric?",
      onConfirm: async () => {
        try {
          await ApiService.deleteFinancialMetric(metricId);
          const refreshed = await ApiService.getInvestInstruments();
          setInstruments(refreshed);
        } catch {
          Alert.alert("Error", "Failed to delete financial metric.");
        }
      },
    });
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Metrics</Text>

      {/* SEARCH – zawsze pełna szerokość */}
      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <Text style={globalStyles.label}>Search investment instrument</Text>
        <TextInput
          style={globalStyles.input}
          placeholder="Name, ticker or ISIN..."
          placeholderTextColor={COLORS.textGrey}
          value={search}
          onChangeText={setSearch}
        />
      </View>

      {/* LIST – 2 kolumny tylko na WEB */}
      <View style={globalStyles.webTwoColumnWrapper}>
        {filteredInstruments.map((i) => (
          <View
            key={i.id}
            style={[
              globalStyles.card,
              globalStyles.webTwoColumnCard,
              spacing.mb4,
            ]}
          >
            {/* HEADER */}
            <View style={[globalStyles.row, globalStyles.spaceBetween]}>
              <View>
                <Text style={globalStyles.cardTitle}>{i.name}</Text>
                <Text style={globalStyles.textSmall}>
                  {i.ticker} • {i.isin}
                </Text>
              </View>

              {/* ICONS – prawy górny róg */}
              <View style={globalStyles.row}>
                {/* EDIT or ADD */}
                {i.financialMetricId ? (
                  <TouchableOpacity
                    style={spacing.mr3}
                    onPress={() =>
                      router.push({
                        pathname: ROUTES.EDIT_FINANCIAL_METRIC,
                        params: { metricId: i.financialMetricId },
                      })
                    }
                  >
                    <Ionicons name="pencil" size={22} color={COLORS.primary} />
                  </TouchableOpacity>
                ) : (
                  <TouchableOpacity
                    style={spacing.mr3}
                    onPress={() =>
                      router.push({
                        pathname: ROUTES.ADD_FINANCIAL_METRIC,
                        params: { instrumentId: i.id },
                      })
                    }
                  >
                    <Ionicons name="add" size={26} color={COLORS.primary} />
                  </TouchableOpacity>
                )}

                {/* DELETE */}
                {i.financialMetricId && (
                  <TouchableOpacity
                    onPress={() => handleDeleteMetric(i.financialMetricId)}
                  >
                    <Ionicons name="trash" size={22} color={COLORS.error} />
                  </TouchableOpacity>
                )}
              </View>
            </View>

            {/* AUTO IMPORT BUTTON */}
            <View style={spacing.mt3}>
              <TouchableOpacity
                style={[globalStyles.button, { width: "100%" }]}
                disabled={importingId === i.id}
                onPress={() => handleAutoImport(i.id)}
              >
                {importingId === i.id ? (
                  <ActivityIndicator color="#fff" />
                ) : (
                  <Text style={globalStyles.buttonText}>
                    {i.financialMetricId
                      ? "Automatically update Financial Metrics"
                      : "Automatically load Financial Metrics"}
                  </Text>
                )}
              </TouchableOpacity>
            </View>
          </View>
        ))}
      </View>

      {!loading && filteredInstruments.length === 0 && (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <Text style={[globalStyles.text, { textAlign: "center" }]}>
            No investment instruments found.
          </Text>
        </View>
      )}
    </ScrollView>
  );
};

export default EditFinancialMetricListScreen;
