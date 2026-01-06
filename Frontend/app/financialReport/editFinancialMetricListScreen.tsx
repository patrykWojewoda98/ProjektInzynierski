import { useLocalSearchParams } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { Picker } from "@react-native-picker/picker";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";

const EditFinancialMetricListScreen = () => {
  const { instrumentId } = useLocalSearchParams();

  const [instruments, setInstruments] = useState([]);
  const [selectedInstrumentId, setSelectedInstrumentId] = useState(
    instrumentId ? Number(instrumentId) : "all"
  );

  const router = useRouter();
  const [reports, setReports] = useState([]);
  const [loading, setLoading] = useState(true);
  const getInstrumentName = (id) =>
    instruments.find((i) => i.id === id)?.name ?? "Unknown instrument";
  const selectedInstrument =
    selectedInstrumentId === "all"
      ? null
      : instruments.find((i) => i.id === selectedInstrumentId);

  const [importing, setImporting] = useState(false);

  const handleDeleteReport = async (id) => {
    try {
      await ApiService.deleteFinancialReport(id);
      setReports((prev) => prev.filter((r) => r.id !== id));
    } catch {
      Alert.alert("Error", "Failed to delete financial report.");
    }
  };
  const loadReports = async () => {
    setLoading(true);

    try {
      let data;
      if (selectedInstrumentId === "all") {
        data = await ApiService.getFinancialReports();
      } else {
        data = await ApiService.getFinancialReportsByInvestInstrumentId(
          selectedInstrumentId
        );
      }
      setReports(data);
    } finally {
      setLoading(false);
    }
  };
  // 📥 LOAD INSTRUMENTS
  useEffect(() => {
    const loadInstruments = async () => {
      const data = await ApiService.getInvestInstruments();
      setInstruments(data);
    };
    loadInstruments();
  }, []);

  // 📥 LOAD REPORTS
  useEffect(() => {
    loadReports();
  }, [selectedInstrumentId]);

  const handleImportReports = async () => {
    if (!selectedInstrument) {
      Alert.alert(
        "Select instrument",
        "Please select an investment instrument."
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

      await loadReports(); // ⬅️ patrz PROBLEM 2
    } catch (error: any) {
      Alert.alert(
        "Import failed",
        error?.response?.data ?? "Failed to import reports."
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
          <Text
            style={[globalStyles.title, spacing.mb3, { textAlign: "center" }]}
          >
            No financial reports available.
          </Text>
        </View>
      )}
      <View style={[globalStyles.card, globalStyles.fullWidth]}>
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
          style={[globalStyles.button, { backgroundColor: COLORS.primary }]}
          onPress={() => {
            if (selectedInstrumentId === "all") {
              Alert.alert(
                "Select instrument",
                "Please select an investment instrument first."
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
      {/* 📄 REPORT LIST */}
      {!loading &&
        reports.map((r) => (
          <View
            key={r.id}
            style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}
          >
            {/* 🔹 INSTRUMENT NAME */}
            <Text style={[globalStyles.cardTitle, spacing.mb1]}>
              {getInstrumentName(r.investInstrumentId)}
            </Text>

            {/* 🔹 PERIOD */}
            <Text style={[globalStyles.cardTitle, spacing.mb2]}>
              Period: {r.period}
            </Text>

            {/* 🔹 DATA */}
            <Text style={globalStyles.text}>Revenue: {r.revenue}</Text>
            <Text style={globalStyles.text}>Net Income: {r.netIncome}</Text>
            <Text style={globalStyles.text}>EPS: {r.eps}</Text>
            <Text style={globalStyles.text}>Assets: {r.assets}</Text>
            <Text style={globalStyles.text}>Liabilities: {r.liabilities}</Text>

            {/* 🔹 ACTIONS */}
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

              <TouchableOpacity
                onPress={() =>
                  Alert.alert(
                    "Confirm delete",
                    "Delete this financial report?",
                    [
                      { text: "Cancel", style: "cancel" },
                      {
                        text: "Delete",
                        style: "destructive",
                        onPress: () => handleDeleteReport(r.id),
                      },
                    ]
                  )
                }
              >
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
            </View>
          </View>
        ))}
    </ScrollView>
  );
};

export default EditFinancialMetricListScreen;
