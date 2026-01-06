import { useEffect, useMemo, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import ApiService from "../../services/api";
import { ROUTES } from "../../routes";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";

const EditFinancialMetricListScreen = () => {
  const router = useRouter();

  const [instruments, setInstruments] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");

  useEffect(() => {
    const load = async () => {
      try {
        const data = await ApiService.getInvestInstruments();
        setInstruments(data);
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
        i.isin?.toLowerCase().includes(q)
    );
  }, [search, instruments]);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Metrics</Text>

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

      {filteredInstruments.map((i) => (
        <View
          key={i.id}
          style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}
        >
          <Text style={[globalStyles.cardTitle, spacing.mb1]}>{i.name}</Text>
          <Text style={globalStyles.text}>
            {i.ticker} • {i.isin}
          </Text>

          <View style={[spacing.mt3, { width: "100%", alignItems: "center" }]}>
            <TouchableOpacity
              style={[
                globalStyles.button,
                { width: "100%", alignSelf: "center" },
              ]}
              onPress={() =>
                router.push({
                  pathname: i.financialMetricId
                    ? ROUTES.EDIT_FINANCIAL_METRIC
                    : ROUTES.ADD_FINANCIAL_METRIC,
                  params: {
                    instrumentId: i.id,
                    metricId: i.financialMetricId ?? null,
                  },
                })
              }
            >
              <Text
                style={[
                  globalStyles.buttonText,
                  { fontSize: 18, textAlign: "center" },
                ]}
              >
                {i.financialMetricId
                  ? "Edit Financial Metric"
                  : "Add Financial Metric"}
              </Text>
            </TouchableOpacity>
          </View>
        </View>
      ))}

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
