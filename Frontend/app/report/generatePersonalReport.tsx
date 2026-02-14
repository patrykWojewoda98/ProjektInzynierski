import { Picker } from "@react-native-picker/picker";
import * as FileSystem from "expo-file-system/legacy";
import { useRouter } from "expo-router";
import * as Sharing from "expo-sharing";
import { useCallback, useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Platform,
  ScrollView,
  Switch,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { AuthContext } from "../_layout";

const METRIC_FIELDS = [
  { key: "PE", label: "P/E" },
  { key: "PB", label: "P/B" },
  { key: "ROE", label: "ROE (%)" },
  { key: "DebtToEquity", label: "Debt/Equity" },
  { key: "DividendYield", label: "Dividend Yield (%)" },
] as const;

const GeneratePersonalReport = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);

  const [instruments, setInstruments] = useState<
    { id: number; name: string }[]
  >([]);
  const [loading, setLoading] = useState(true);
  const [loadingDetails, setLoadingDetails] = useState(false);
  const [generating, setGenerating] = useState(false);
  const [savingToDevice, setSavingToDevice] = useState(false);

  const [selectedInstrumentId, setSelectedInstrumentId] = useState<
    number | null
  >(null);
  const [includeInstrumentInfo, setIncludeInstrumentInfo] = useState(true);
  const [includeFinancialMetrics, setIncludeFinancialMetrics] = useState(true);
  const [selectedMetricFields, setSelectedMetricFields] = useState<string[]>([
    "PE",
    "PB",
    "ROE",
    "DebtToEquity",
    "DividendYield",
  ]);
  const [includeFinancialReports, setIncludeFinancialReports] = useState(false);
  const [reports, setReports] = useState<{ id: number; period: string }[]>([]);
  const [selectedReportIds, setSelectedReportIds] = useState<number[]>([]);
  const [includePortfolioComposition, setIncludePortfolioComposition] =
    useState(true);
  const [hasMetric, setHasMetric] = useState(false);

  useEffect(() => {
    const load = async () => {
      try {
        const instrs = await ApiService.getInvestInstruments();
        setInstruments(
          (instrs || []).map((i: any) => ({ id: i.id, name: i.name })),
        );
      } catch (e) {
        console.error(e);
        Alert.alert("Error", "Failed to load instruments.");
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const loadInstrumentDetails = useCallback(async (instrumentId: number) => {
    setLoadingDetails(true);
    try {
      const [instrument, reportsData] = await Promise.all([
        ApiService.getInvestInstrumentById(instrumentId),
        ApiService.getFinancialReportsByInvestInstrumentId(instrumentId),
      ]);
      setReports(
        (reportsData || []).map((r: any) => ({
          id: r.id,
          period: r.period || `Report #${r.id}`,
        })),
      );
      setSelectedReportIds([]);

      if (instrument?.financialMetricId) {
        try {
          await ApiService.getFinancialMetricById(instrument.financialMetricId);
          setHasMetric(true);
          setSelectedMetricFields([
            "PE",
            "PB",
            "ROE",
            "DebtToEquity",
            "DividendYield",
          ]);
        } catch {
          setHasMetric(false);
        }
      } else {
        setHasMetric(false);
      }
    } catch (e) {
      console.error(e);
      setReports([]);
      setHasMetric(false);
    } finally {
      setLoadingDetails(false);
    }
  }, []);

  useEffect(() => {
    if (selectedInstrumentId != null)
      loadInstrumentDetails(selectedInstrumentId);
    else {
      setReports([]);
      setSelectedReportIds([]);
      setHasMetric(false);
    }
  }, [selectedInstrumentId, loadInstrumentDetails]);

  const toggleMetricField = (key: string) => {
    setSelectedMetricFields((prev) =>
      prev.includes(key) ? prev.filter((k) => k !== key) : [...prev, key],
    );
  };

  const toggleReportId = (id: number) => {
    setSelectedReportIds((prev) =>
      prev.includes(id) ? prev.filter((r) => r !== id) : [...prev, id],
    );
  };

  const canGenerate =
    selectedInstrumentId != null &&
    (includeInstrumentInfo ||
      (includeFinancialMetrics &&
        (!hasMetric || selectedMetricFields.length > 0)) ||
      (includeFinancialReports &&
        (reports.length === 0 || selectedReportIds.length > 0)) ||
      includePortfolioComposition);

  const handleGenerate = async () => {
    if (!user?.id || !canGenerate) {
      Alert.alert(
        "Invalid options",
        "Select an instrument and at least one section. If including financial reports, select at least one period.",
      );
      return;
    }
    if (
      includeFinancialReports &&
      reports.length > 0 &&
      selectedReportIds.length === 0
    ) {
      Alert.alert(
        "Select periods",
        "You have reports for this instrument. Select at least one period to include, or turn off Financial reports.",
      );
      return;
    }
    setGenerating(true);
    try {
      const fileUri = await ApiService.generatePersonalReportPdf({
        clientId: user.id,
        investInstrumentId: selectedInstrumentId ?? undefined,
        includeInstrumentInfo,
        includeFinancialMetrics,
        includedMetricFields:
          includeFinancialMetrics && selectedMetricFields.length > 0
            ? selectedMetricFields
            : undefined,
        includeFinancialReports,
        includedFinancialReportIds: includeFinancialReports
          ? selectedReportIds
          : undefined,
        includePortfolioComposition,
      });
      if (Platform.OS !== "web" && fileUri) {
        const canShare = await Sharing.isAvailableAsync();
        if (canShare) await Sharing.shareAsync(fileUri);
        else Alert.alert("Done", "Report saved.");
      } else if (Platform.OS === "web") {
        Alert.alert("Done", "Report opened in a new tab.");
      }
    } catch (e) {
      console.error(e);
      Alert.alert("Error", "Failed to generate report.");
    } finally {
      setGenerating(false);
    }
  };

  const handleSavePdfToDevice = async () => {
    if (!user?.id || !canGenerate) return;
    if (
      includeFinancialReports &&
      reports.length > 0 &&
      selectedReportIds.length === 0
    ) {
      Alert.alert(
        "Select periods",
        "You have reports for this instrument. Select at least one period to include, or turn off Financial reports.",
      );
      return;
    }
    setSavingToDevice(true);
    try {
      const fileUri = await ApiService.generatePersonalReportPdf({
        clientId: user.id,
        investInstrumentId: selectedInstrumentId ?? undefined,
        includeInstrumentInfo,
        includeFinancialMetrics,
        includedMetricFields:
          includeFinancialMetrics && selectedMetricFields.length > 0
            ? selectedMetricFields
            : undefined,
        includeFinancialReports,
        includedFinancialReportIds: includeFinancialReports
          ? selectedReportIds
          : undefined,
        includePortfolioComposition,
      });
      const permissions =
        await FileSystem.StorageAccessFramework.requestDirectoryPermissionsAsync();
      if (!permissions.granted) {
        Alert.alert("Cancelled", "Folder access is needed to save the PDF.");
        return;
      }
      const fileName = `Personal_Report_${Date.now()}.pdf`;
      const newUri = await FileSystem.StorageAccessFramework.createFileAsync(
        permissions.directoryUri,
        fileName,
        "application/pdf",
      );
      const pdfBase64 = await FileSystem.readAsStringAsync(fileUri, {
        encoding: FileSystem.EncodingType.Base64,
      });
      await FileSystem.writeAsStringAsync(newUri, pdfBase64, {
        encoding: FileSystem.EncodingType.Base64,
      });
      Alert.alert("Saved", "PDF saved successfully to the selected folder.");
    } catch (e) {
      console.error(e);
      Alert.alert("Error", "Failed to save PDF to device.");
    } finally {
      setSavingToDevice(false);
    }
  };

  if (loading) {
    return (
      <View style={[globalStyles.centerContainer]}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView
      contentContainerStyle={[globalStyles.scrollContainer, spacing.p4]}
    >
      <Text style={[globalStyles.header, spacing.mb4]}>
        Generate Personal Report
      </Text>
      <Text style={[globalStyles.text, spacing.mb4]}>
        Choose an instrument first, then select which data to include in the
        PDF.
      </Text>

      {/* 1. Instrument dropdown at top – same style as Financial Reports */}
      <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
        <Text style={[globalStyles.label, spacing.mb2]}>
          Investment Instrument
        </Text>
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
            selectedValue={selectedInstrumentId ?? ""}
            onValueChange={(v) =>
              setSelectedInstrumentId(v ? Number(v) : null)
            }
            style={[
              globalStyles.pickerText,
              globalStyles.pickerWeb,
              { flex: 1 },
            ]}
            dropdownIconColor={COLORS.textGrey}
            prompt="Select instrument"
          >
            <Picker.Item label="— Select instrument —" value="" />
            {instruments.map((i) => (
              <Picker.Item key={i.id} label={i.name} value={i.id} />
            ))}
          </Picker>
        </View>
        {loadingDetails && (
          <View style={spacing.mt2}>
            <ActivityIndicator size="small" />
          </View>
        )}
      </View>

      {selectedInstrumentId == null && (
        <Text style={[globalStyles.textSmall, spacing.mb4]}>
          Select an instrument above to choose metrics and report periods.
        </Text>
      )}

      {selectedInstrumentId != null && (
        <>
          {/* 2. Include instrument details */}
          <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
            <Text style={[globalStyles.label, spacing.mb2]}>
              Instrument details
            </Text>
            <View style={[globalStyles.row, globalStyles.spaceBetween]}>
              <Text style={globalStyles.text}>
                Include instrument details in report
              </Text>
              <Switch
                value={includeInstrumentInfo}
                onValueChange={setIncludeInstrumentInfo}
              />
            </View>
          </View>

          {/* 3. Financial metrics – which fields */}
          <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
            <View
              style={[globalStyles.row, globalStyles.spaceBetween, spacing.mb2]}
            >
              <Text style={globalStyles.label}>Financial metrics</Text>
              <Switch
                value={includeFinancialMetrics}
                onValueChange={setIncludeFinancialMetrics}
              />
            </View>
            {includeFinancialMetrics && (
              <>
                <Text style={[globalStyles.textSmall, spacing.mb2]}>
                  Choose which metrics to include:
                </Text>
                {hasMetric ? (
                  METRIC_FIELDS.map((f) => (
                    <View
                      key={f.key}
                      style={[
                        globalStyles.row,
                        globalStyles.spaceBetween,
                        spacing.py1,
                      ]}
                    >
                      <Text style={globalStyles.text}>{f.label}</Text>
                      <Switch
                        value={selectedMetricFields.includes(f.key)}
                        onValueChange={() => toggleMetricField(f.key)}
                      />
                    </View>
                  ))
                ) : (
                  <Text style={globalStyles.textSmall}>
                    No financial metrics for this instrument.
                  </Text>
                )}
              </>
            )}
          </View>

          {/* 4. Financial reports – which periods */}
          <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
            <View
              style={[globalStyles.row, globalStyles.spaceBetween, spacing.mb2]}
            >
              <Text style={globalStyles.label}>Financial reports</Text>
              <Switch
                value={includeFinancialReports}
                onValueChange={setIncludeFinancialReports}
              />
            </View>
            {includeFinancialReports && (
              <>
                <Text style={[globalStyles.textSmall, spacing.mb2]}>
                  Choose which report periods to include:
                </Text>
                {reports.length === 0 ? (
                  <Text style={globalStyles.textSmall}>
                    No financial reports for this instrument.
                  </Text>
                ) : (
                  reports.map((r) => (
                    <View
                      key={r.id}
                      style={[
                        globalStyles.row,
                        globalStyles.spaceBetween,
                        spacing.py1,
                      ]}
                    >
                      <Text style={globalStyles.text}>{r.period}</Text>
                      <Switch
                        value={selectedReportIds.includes(r.id)}
                        onValueChange={() => toggleReportId(r.id)}
                      />
                    </View>
                  ))
                )}
              </>
            )}
          </View>

          {/* 5. Portfolio composition */}
          <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
            <Text style={[globalStyles.label, spacing.mb2]}>
              Portfolio composition
            </Text>
            <View style={[globalStyles.row, globalStyles.spaceBetween]}>
              <Text style={globalStyles.text}>
                Include full wallet (all positions)
              </Text>
              <Switch
                value={includePortfolioComposition}
                onValueChange={setIncludePortfolioComposition}
              />
            </View>
          </View>
        </>
      )}

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={handleGenerate}
        disabled={!canGenerate || generating || loadingDetails}
      >
        {generating ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>
            {Platform.OS === "web"
              ? "Generate report (PDF)"
              : "Share report (PDF)"}
          </Text>
        )}
      </TouchableOpacity>

      {Platform.OS !== "web" && (
        <TouchableOpacity
          style={[
            globalStyles.button,
            spacing.mb4,
            { backgroundColor: COLORS.mediumGrey },
          ]}
          onPress={handleSavePdfToDevice}
          disabled={!canGenerate || savingToDevice || loadingDetails}
        >
          {savingToDevice ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Save PDF to device</Text>
          )}
        </TouchableOpacity>
      )}

      <TouchableOpacity onPress={() => router.push(ROUTES.MAIN_MENU)}>
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default GeneratePersonalReport;
