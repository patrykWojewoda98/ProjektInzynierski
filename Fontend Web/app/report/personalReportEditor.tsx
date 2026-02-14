import { Picker } from "@react-native-picker/picker";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  NativeSyntheticEvent,
  NativeScrollEvent,
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
import { AuthContext } from "../_layout";

const FONT_DEFAULT = "default";
const FONT_OPTIONS = [
  { value: FONT_DEFAULT, label: "Default" },
  { value: "Lato", label: "Lato" },
  { value: "Helvetica", label: "Helvetica" },
  { value: "Times New Roman", label: "Times New Roman" },
  { value: "Arial", label: "Arial" },
];

const FONT_SIZE_OPTIONS = [8, 10, 12, 14];

const PersonalReportEditor = () => {
  const router = useRouter();
  const params = useLocalSearchParams<{
    selectedInstrumentId?: string;
    includeInstrumentInfo?: string;
    includeFinancialMetrics?: string;
    metricFields?: string;
    includeFinancialReports?: string;
    reportIds?: string;
    includePortfolioComposition?: string;
  }>();
  const { user } = useContext(AuthContext);

  const scrollRef = useRef<ScrollView>(null);
  const scrollOffsetY = useRef(0);

  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);
  const [preview, setPreview] = useState<string>("");
  const [introText, setIntroText] = useState("");
  const [outroText, setOutroText] = useState("");
  const [fontFamily, setFontFamily] = useState(FONT_DEFAULT);
  const [fontSize, setFontSize] = useState<number>(10);

  const { instrumentId, includeInstrumentInfo, includeFinancialMetrics, includedMetricFields, includeFinancialReports, includedReportIds, includePortfolioComposition } = useMemo(() => {
    const id = params.selectedInstrumentId
      ? parseInt(params.selectedInstrumentId, 10)
      : null;
    let metricFields: string[] = [];
    let reportIds: number[] = [];
    try {
      if (params.metricFields) metricFields = JSON.parse(params.metricFields);
    } catch {
      // ignore
    }
    try {
      if (params.reportIds) reportIds = JSON.parse(params.reportIds);
    } catch {
      // ignore
    }
    return {
      instrumentId: id,
      includeInstrumentInfo: params.includeInstrumentInfo === "true",
      includeFinancialMetrics: params.includeFinancialMetrics === "true",
      includedMetricFields: metricFields,
      includeFinancialReports: params.includeFinancialReports === "true",
      includedReportIds: reportIds,
      includePortfolioComposition: params.includePortfolioComposition === "true",
    };
  }, [
    params.selectedInstrumentId,
    params.includeInstrumentInfo,
    params.includeFinancialMetrics,
    params.metricFields,
    params.includeFinancialReports,
    params.reportIds,
    params.includePortfolioComposition,
  ]);

  const loadPreview = useCallback(async () => {
    if (!instrumentId || !user?.id) return;
    setLoading(true);
    try {
      const parts: string[] = [];
      if (includeInstrumentInfo) {
        const instrument = await ApiService.getInvestInstrumentById(
          instrumentId,
        );
        parts.push(
          `Instrument: ${instrument?.name ?? ""} (${instrument?.ticker ?? ""})`,
        );
      }
      if (includeFinancialMetrics && includedMetricFields.length > 0) {
        parts.push(`Financial metrics: ${includedMetricFields.join(", ")}`);
      }
      if (includeFinancialReports) {
        if (includedReportIds.length > 0) {
          parts.push(`Financial reports: ${includedReportIds.length} period(s)`);
        } else {
          parts.push("Financial reports: (all or none)");
        }
      }
      if (includePortfolioComposition) {
        parts.push("Portfolio composition: full wallet");
      }
      setPreview(parts.join("\n") || "No sections selected.");
    } catch (e) {
      console.error(e);
      setPreview("Could not load preview.");
    } finally {
      setLoading(false);
    }
  }, [
    instrumentId,
    user?.id,
    includeInstrumentInfo,
    includeFinancialMetrics,
    includedMetricFields,
    includeFinancialReports,
    includedReportIds,
    includePortfolioComposition,
  ]);

  useEffect(() => {
    loadPreview();
  }, [loadPreview]);

  useEffect(() => {
    const t = requestAnimationFrame(() => {
      scrollRef.current?.scrollTo({ y: scrollOffsetY.current, animated: false });
    });
    return () => cancelAnimationFrame(t);
  }, [introText, outroText]);

  const handleGenerate = async () => {
    if (!user?.id || !instrumentId) {
      Alert.alert("Error", "Missing user or instrument.");
      return;
    }
    setGenerating(true);
    try {
      await ApiService.generatePersonalReportPdf({
        clientId: user.id,
        investInstrumentId: instrumentId,
        includeInstrumentInfo,
        includeFinancialMetrics,
        includedMetricFields:
          includeFinancialMetrics && includedMetricFields.length > 0
            ? includedMetricFields
            : undefined,
        includeFinancialReports,
        includedFinancialReportIds: includeFinancialReports
          ? includedReportIds
          : undefined,
        includePortfolioComposition,
        customIntroText: introText.trim() || undefined,
        customOutroText: outroText.trim() || undefined,
        fontFamily: fontFamily && fontFamily !== FONT_DEFAULT ? fontFamily : undefined,
        fontSize: fontSize,
      });
      Alert.alert("Done", "Report opened in a new tab.");
    } catch (e) {
      console.error(e);
      Alert.alert("Error", "Failed to generate report.");
    } finally {
      setGenerating(false);
    }
  };

  if (!instrumentId) {
    return (
      <View style={[globalStyles.centerContainer, spacing.p4]}>
        <Text style={[globalStyles.text, spacing.mb4]}>
          Open the editor from the Generate Personal Report screen after
          selecting an instrument and options.
        </Text>
        <TouchableOpacity
          style={[globalStyles.button, spacing.mb4]}
          onPress={() => router.push(ROUTES.GENERATE_PERSONAL_REPORT)}
        >
          <Text style={globalStyles.buttonText}>Go to Generate Report</Text>
        </TouchableOpacity>
        <TouchableOpacity onPress={() => router.push(ROUTES.MAIN_MENU)}>
          <Text style={globalStyles.link}>Back to Main Menu</Text>
        </TouchableOpacity>
      </View>
    );
  }

  if (loading) {
    return (
      <View style={[globalStyles.centerContainer]}>
        <ActivityIndicator size="large" color={COLORS.primary} />
      </View>
    );
  }

  return (
    <ScrollView
      ref={scrollRef}
      onScroll={(e: NativeSyntheticEvent<NativeScrollEvent>) => {
        scrollOffsetY.current = e.nativeEvent.contentOffset.y;
      }}
      scrollEventThrottle={32}
      contentContainerStyle={[globalStyles.scrollContainer, spacing.p4]}
    >
      <Text style={[globalStyles.header, spacing.mb4]}>
        Personal Report Editor
      </Text>
      <Text style={[globalStyles.text, spacing.mb4]}>
        Data included in this report (loaded from your choices). Add your own
        text and choose font, then generate the PDF.
      </Text>

      <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
        <Text style={[globalStyles.label, spacing.mb2]}>Report content</Text>
        <Text style={globalStyles.textSmall}>{preview}</Text>
      </View>

      <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
        <Text style={[globalStyles.label, spacing.mb2]}>
          Your text (appears at the start of the report)
        </Text>
        <TextInput
          style={[
            globalStyles.input,
            { minHeight: 80, textAlignVertical: "top" },
          ]}
          placeholder="Optional intro or note..."
          placeholderTextColor={COLORS.textGrey}
          multiline
          value={introText}
          onChangeText={setIntroText}
        />
      </View>

      <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
        <Text style={[globalStyles.label, spacing.mb2]}>Font</Text>
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
            selectedValue={fontFamily}
            onValueChange={(v) => setFontFamily(v)}
            style={[globalStyles.pickerText, globalStyles.pickerWeb, { flex: 1 }]}
            dropdownIconColor={COLORS.textGrey}
          >
            {FONT_OPTIONS.map((o) => (
              <Picker.Item
                key={o.value}
                label={o.label}
                value={o.value}
              />
            ))}
          </Picker>
        </View>
      </View>

      <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
        <Text style={[globalStyles.label, spacing.mb2]}>Font size</Text>
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
            selectedValue={fontSize}
            onValueChange={(v) => setFontSize(Number(v))}
            style={[globalStyles.pickerText, globalStyles.pickerWeb, { flex: 1 }]}
            dropdownIconColor={COLORS.textGrey}
          >
            {FONT_SIZE_OPTIONS.map((n) => (
              <Picker.Item key={n} label={`${n} pt`} value={n} />
            ))}
          </Picker>
        </View>
      </View>

      <View style={[globalStyles.card, spacing.p4, spacing.mb4]}>
        <Text style={[globalStyles.label, spacing.mb2]}>
          Your text (appears at the end of the report)
        </Text>
        <TextInput
          style={[
            globalStyles.input,
            { minHeight: 80, textAlignVertical: "top" },
          ]}
          placeholder="Optional outro or note..."
          placeholderTextColor={COLORS.textGrey}
          multiline
          value={outroText}
          onChangeText={setOutroText}
        />
      </View>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={handleGenerate}
        disabled={generating}
      >
        {generating ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Generate report (PDF)</Text>
        )}
      </TouchableOpacity>

      <TouchableOpacity
        style={spacing.mb2}
        onPress={() => router.push(ROUTES.GENERATE_PERSONAL_REPORT)}
      >
        <Text style={globalStyles.link}>Back to Generate Report</Text>
      </TouchableOpacity>
      <TouchableOpacity onPress={() => router.push(ROUTES.MAIN_MENU)}>
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default PersonalReportEditor;
