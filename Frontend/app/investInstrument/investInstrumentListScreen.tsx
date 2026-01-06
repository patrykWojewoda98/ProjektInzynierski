import { Picker } from "@react-native-picker/picker";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";

const InvestmentInstrumentListScreen = () => {
  const router = useRouter();

  const [regions, setRegions] = useState([]);
  const [sectors, setSectors] = useState([]);
  const [types, setTypes] = useState([]);
  const [items, setItems] = useState([]);

  const [regionId, setRegionId] = useState(null);
  const [sectorId, setSectorId] = useState(null);
  const [typeId, setTypeId] = useState(null);

  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  // 🔐 AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  // 📥 FILTERS
  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      const [r, s, t] = await Promise.all([
        ApiService.getAllRegions(),
        ApiService.getSectors(),
        ApiService.getAllInvestmentTypes(),
      ]);
      setRegions(r);
      setSectors(s);
      setTypes(t);
    };

    load();
  }, [isReady]);

  // 📥 INSTRUMENTS
  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      setLoading(true);

      let data;
      if (regionId)
        data = await ApiService.getInvestInstrumentsByRegion(regionId);
      else if (sectorId)
        data = await ApiService.getInvestInstrumentsBySector(sectorId);
      else if (typeId)
        data = await ApiService.getInvestInstrumentsByType(typeId);
      else data = await ApiService.getInvestInstruments();

      setItems(data);
      setLoading(false);
    };

    load();
  }, [regionId, sectorId, typeId, isReady]);

  const handleDelete = (id) => {
    Alert.alert("Confirm delete", "Delete this instrument?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          await ApiService.deleteInvestInstrument(id);
          setItems((p) => p.filter((x) => x.id !== id));
        },
      },
    ]);
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Investment Instruments
      </Text>

      {/* ➕ ADD */}
      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_INVEST_INSTRUMENT)}
      >
        <Text style={globalStyles.buttonText}>Add new instrument</Text>
      </TouchableOpacity>

      {/* REGION */}
      <Text style={globalStyles.label}>Region</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          selectedValue={regionId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          onValueChange={(v) => {
            setRegionId(v);
            setSectorId(null);
            setTypeId(null);
          }}
        >
          <Picker.Item label="All regions" value={null} />
          {regions.map((r) => (
            <Picker.Item key={r.id} label={r.name} value={r.id} />
          ))}
        </Picker>
      </View>

      {/* SECTOR */}
      <Text style={globalStyles.label}>Sector</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          selectedValue={sectorId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          onValueChange={(v) => {
            setSectorId(v);
            setRegionId(null);
            setTypeId(null);
          }}
        >
          <Picker.Item label="All sectors" value={null} />
          {sectors.map((s) => (
            <Picker.Item key={s.id} label={s.name} value={s.id} />
          ))}
        </Picker>
      </View>

      {/* TYPE */}
      <Text style={globalStyles.label}>Type</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          selectedValue={typeId}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          onValueChange={(v) => {
            setTypeId(v);
            setRegionId(null);
            setSectorId(null);
          }}
        >
          <Picker.Item label="All types" value={null} />
          {types.map((t) => (
            <Picker.Item key={t.id} label={t.typeName} value={t.id} />
          ))}
        </Picker>
      </View>

      {/* LIST */}
      <View style={[spacing.mt3, globalStyles.fullWidth]}>
        {items.map((i) => (
          <View
            key={i.id}
            style={[globalStyles.card, spacing.mb4, { width: "100%" }]}
          >
            <Text style={globalStyles.cardTitle}>{i.ticker}</Text>
            <Text style={globalStyles.text}>{i.name}</Text>
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
                    pathname: ROUTES.EDIT_INVEST_INSTRUMENT,
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

            <View
              style={[spacing.mt2, { width: "100%", alignItems: "center" }]}
            >
              <TouchableOpacity
                style={[
                  globalStyles.button,
                  { width: "100%", alignSelf: "center" },
                ]}
                onPress={async () => {
                  const instrument = await ApiService.getInvestInstrumentById(
                    i.id
                  );

                  router.push({
                    pathname: instrument.financialMetricId
                      ? ROUTES.EDIT_FINANCIAL_METRIC
                      : ROUTES.ADD_FINANCIAL_METRIC,
                    params: {
                      instrumentId: i.id,
                      metricId: instrument.financialMetricId ?? null,
                    },
                  });
                }}
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

            <View
              style={[spacing.mt2, { width: "100%", alignItems: "center" }]}
            >
              <TouchableOpacity
                style={[
                  globalStyles.button,
                  {
                    width: "100%",
                    alignSelf: "center",
                    backgroundColor: COLORS.primary,
                  },
                ]}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_FINANCIAL_REPORT_LIST,
                    params: { instrumentId: i.id },
                  })
                }
              >
                <Text
                  style={[
                    globalStyles.buttonText,
                    { fontSize: 18, textAlign: "center" },
                  ]}
                >
                  Financial Reports
                </Text>
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

export default InvestmentInstrumentListScreen;
