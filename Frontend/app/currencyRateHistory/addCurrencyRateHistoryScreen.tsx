import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
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
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const AddCurrencyRateHistoryScreen = () => {
  const { pairId } = useLocalSearchParams();
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns();

  const parsedPairId = Number(Array.isArray(pairId) ? pairId[0] : pairId);

  const [openRate, setOpenRate] = useState("");
  const [highRate, setHighRate] = useState("");
  const [lowRate, setLowRate] = useState("");
  const [closeRate, setCloseRate] = useState("");
  const [date, setDate] = useState(new Date().toISOString().split("T")[0]);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      setLoading(false);
    };
    check();
  }, []);

  const parse = (v: string) => (v === "" ? null : Number(v));

  const handleSave = async () => {
    if (!parsedPairId || isNaN(parsedPairId)) {
      Alert.alert("Error", "Invalid currency pair.");
      return;
    }

    const o = parse(openRate);
    const h = parse(highRate);
    const l = parse(lowRate);
    const c = parse(closeRate);

    if (c === null || c <= 0) {
      Alert.alert("Validation error", "Close rate must be greater than zero.");
      return;
    }

    if (
      (h !== null && l !== null && h < l) ||
      (h !== null && c !== null && h < c) ||
      (l !== null && c !== null && l > c)
    ) {
      Alert.alert(
        "Validation error",
        "Rates must satisfy: High ≥ Close ≥ Low.",
      );
      return;
    }

    setSaving(true);

    try {
      await ApiService.createCurrencyRateHistory({
        currencyPairId: parsedPairId,
        date,
        closeRate: c,
        openRate: o,
        highRate: h,
        lowRate: l,
      });

      Alert.alert("Success", "Currency rate added.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to create currency rate.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Add Currency Rate</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Open Rate</Text>
          <TextInput
            style={globalStyles.input}
            value={openRate}
            onChangeText={setOpenRate}
            keyboardType="numeric"
          />
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>High Rate</Text>
          <TextInput
            style={globalStyles.input}
            value={highRate}
            onChangeText={setHighRate}
            keyboardType="numeric"
          />
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Low Rate</Text>
          <TextInput
            style={globalStyles.input}
            value={lowRate}
            onChangeText={setLowRate}
            keyboardType="numeric"
          />
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.label}>Close Rate *</Text>
          <TextInput
            style={globalStyles.input}
            value={closeRate}
            onChangeText={setCloseRate}
            keyboardType="numeric"
          />
        </View>
      </View>

      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mt2]}>
        <Text style={globalStyles.label}>Date</Text>
        <TextInput
          style={globalStyles.input}
          value={date}
          onChangeText={setDate}
        />
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          spacing.mt4,
          saving && globalStyles.buttonDisabled,
        ]}
        disabled={saving}
        onPress={handleSave}
      >
        {saving ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>Save</Text>
        )}
      </TouchableOpacity>

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default AddCurrencyRateHistoryScreen;
