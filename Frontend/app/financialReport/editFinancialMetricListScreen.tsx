import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
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

const EditFinancialMetricListScreen = () => {
  const { instrumentId } = useLocalSearchParams();
  const router = useRouter();

  const [instruments, setInstruments] = useState<any[]>([]);
  const [reports, setReports] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [importing, setImporting] = useState(false);
  const [instrumentsLoaded, setInstrumentsLoaded] = useState(false);

  const [selectedInstrumentId, setSelectedInstrumentId] = useState<
    number | "all"
  >(instrumentId ? Number(instrumentId) : "all");

  const selectedInstrument =
    selectedInstrumentId === "all"
      ? null
      : instruments.find((i) => i.id === selectedInstrumentId);

  const getInstrumentName = (id: number) =>
    instruments.find((i) => i.id === id)?.name ?? "Unknown instrument";

  // 📥 LOAD INSTRUMENTS
  useEffect(() => {
    const loadInstruments = async () => {
      const data = await ApiService.getInvestInstruments();
      setInstruments(data);
      setInstrumentsLoaded(true);
    };

    loadInstruments();
  }, []);

  // 📥 LOAD REPORTS
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

  const handleDeleteReport = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this financial report?",
      onConfirm: async () => {
        try {
          await ApiService.deleteFinancialReport(id);
          setReports((prev) => prev.filter((r) => r.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete financial report.");
        }
      },
    });
  };

  const handleImportReports = async () => {
    if (!selectedInstrument) {
      Alert.alert(
        "Select instrument",
        "Please select an investment instrument.",
      );
      return;
    }

    if (!selectedInstrument.isin) {
      Alert.alert("Missing ISIN", "This instrument does not have ISIN.");
      return;
    }

    setImporting(true);

    try {
      await ApiService.importFinancialReportsByIsin(selectedInstrument.isin);
      Alert.alert("Success", "Financial reports imported successfully.");
      await loadReports();
    } catch (error: any) {
      Alert.alert(
        "Import failed",
        error?.response?.data ?? "Failed to import reports.",
      );
    } finally {
      setImporting(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Reports</Text>

      {/* 🔽 DROPDOWN */}
      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <Text style={globalStyles.label}>Investment Instrument</Text>
        <Picker
          selectedValue={selectedInstrumentId}
          onValueChange={(value) => setSelectedInstrumentId(value)}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="Show all" value="all" />
          {instruments.map((i) => (
            <Picker.Item key={i.id} label={i.name} value={i.id} />
          ))}
        </Picker>
      </View>

      {/* 🔄 LOADING */}
      {loading && <ActivityIndicator color={COLORS.primary} />}

      {/* ❌ NO REPORTS */}
      {!loading && reports.length === 0 && (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <Text style={[globalStyles.title, { textAlign: "center" }]}>
            No financial reports available.
          </Text>
        </View>
      )}

      {/* 🔘 ACTION BUTTONS */}
      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <TouchableOpacity
          style={[
            globalStyles.button,
            spacing.mb2,
            importing && globalStyles.buttonDisabled,
          ]}
          disabled={importing}
          onPress={handleImportReports}
        >
          {importing ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>
              Try to import reports automatically (ISIN)
            </Text>
          )}
        </TouchableOpacity>

        <TouchableOpacity
          style={globalStyles.button}
          onPress={() => {
            if (selectedInstrumentId === "all") {
              Alert.alert(
                "Select instrument",
                "Please select an investment instrument first.",
              );
              return;
            }

            router.push({
              pathname: ROUTES.ADD_FINANCIAL_REPORT,
              params: { instrumentId: selectedInstrumentId },
            });
          }}
        >
          <Text style={globalStyles.buttonText}>Add report manually</Text>
        </TouchableOpacity>
      </View>

      {/* 📄 REPORT LIST */}
      {!loading &&
        reports.map((r) => (
          <View
            key={r.id}
            style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}
          >
            <Text style={globalStyles.cardTitle}>
              {getInstrumentName(r.investInstrumentId)}
            </Text>

            <Text style={[globalStyles.text, spacing.mb2]}>
              Period: {r.period}
            </Text>

            <Text style={globalStyles.text}>Revenue: {r.revenue}</Text>
            <Text style={globalStyles.text}>Net Income: {r.netIncome}</Text>
            <Text style={globalStyles.text}>EPS: {r.eps}</Text>
            <Text style={globalStyles.text}>Assets: {r.assets}</Text>
            <Text style={globalStyles.text}>Liabilities: {r.liabilities}</Text>

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
                    pathname: ROUTES.EDIT_FINANCIAL_REPORT,
                    params: { id: r.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDeleteReport(r.id)}>
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
            </View>
          </View>
        ))}
    </ScrollView>
  );
};

export default EditFinancialMetricListScreen;
