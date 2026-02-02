import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  View,
  useWindowDimensions,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const FinancialReports = () => {
  const { instrumentId } = useLocalSearchParams();
  const router = useRouter();
  const { width } = useWindowDimensions();

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const columnWidth = `${100 / columns - 4}%`;

  const [instruments, setInstruments] = useState<any[]>([]);
  const [reports, setReports] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [instrumentsLoaded, setInstrumentsLoaded] = useState(false);

  const [selectedInstrumentId, setSelectedInstrumentId] = useState<
    number | "all"
  >(instrumentId ? Number(instrumentId) : "all");

  const getInstrumentName = (id: number) =>
    instruments.find((i) => i.id === id)?.name ?? "Unknown instrument";

  useEffect(() => {
    const loadInstruments = async () => {
      const data = await ApiService.getInvestInstruments();
      setInstruments(data);
      setInstrumentsLoaded(true);
    };
    loadInstruments();
  }, []);

  useEffect(() => {
    if (!instrumentsLoaded) return;
    loadReports();
  }, [selectedInstrumentId, instrumentsLoaded]);

  const loadReports = async () => {
    setLoading(true);
    try {
      const data =
        selectedInstrumentId === "all"
          ? await ApiService.getFinancialReports()
          : await ApiService.getFinancialReportsByInvestInstrumentId(
              selectedInstrumentId,
            );
      setReports(data);
    } finally {
      setLoading(false);
    }
  };

  const renderPicker = () => (
    <View style={[spacing.m2, { width: columnWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>Investment Instrument</Text>
        <View
          style={[
            globalStyles.pickerWrapper,
            globalStyles.pickerWebWrapper,
            {
              flexDirection: "row",
              alignItems: "center",
              paddingHorizontal: 12,
              height: 48,
            },
          ]}
        >
          <Picker
            selectedValue={selectedInstrumentId}
            onValueChange={(v) => setSelectedInstrumentId(v)}
            style={[
              globalStyles.pickerText,
              globalStyles.pickerWeb,
              { flex: 1 },
            ]}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="Show all" value="all" />
            {instruments.map((i) => (
              <Picker.Item key={i.id} label={i.name} value={i.id} />
            ))}
          </Picker>
          <Ionicons
            name="chevron-down"
            size={18}
            color={COLORS.textGrey}
            style={globalStyles.pickerWebArrow}
          />
        </View>
      </View>
    </View>
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Reports</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {renderPicker()}
      </View>

      {loading && <ActivityIndicator color={COLORS.primary} />}

      {!loading && reports.length === 0 && (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <Text style={[globalStyles.title, { textAlign: "center" }]}>
            No financial reports available.
          </Text>
        </View>
      )}

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {!loading &&
          reports.map((r) => (
            <View key={r.id} style={[spacing.m2, { width: columnWidth }]}>
              <View style={globalStyles.card}>
                <Text
                  style={[globalStyles.cardTitle, { color: COLORS.success }]}
                >
                  {getInstrumentName(r.investInstrumentId)}
                </Text>

                <Text style={[globalStyles.text, spacing.mb2]}>
                  Period: {r.period}
                </Text>

                <Text style={globalStyles.text}>Revenue: {r.revenue}</Text>
                <Text style={globalStyles.text}>Net Income: {r.netIncome}</Text>
                <Text style={globalStyles.text}>EPS: {r.eps}</Text>
                <Text style={globalStyles.text}>Assets: {r.assets}</Text>
                <Text style={globalStyles.text}>
                  Liabilities: {r.liabilities}
                </Text>

                <View
                  style={[
                    globalStyles.row,
                    spacing.mt3,
                    { justifyContent: "center" },
                  ]}
                ></View>
              </View>
            </View>
          ))}
      </View>
    </ScrollView>
  );
};

export default FinancialReports;
