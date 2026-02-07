import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import { useEffect, useMemo, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
  useWindowDimensions,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const InvestmentInstrumentListScreen = () => {
  const router = useRouter();
  const { width } = useWindowDimensions();

  const [regions, setRegions] = useState<any[]>([]);
  const [sectors, setSectors] = useState<any[]>([]);
  const [types, setTypes] = useState<any[]>([]);
  const [allItems, setAllItems] = useState<any[]>([]);

  const [regionId, setRegionId] = useState(0);
  const [sectorId, setSectorId] = useState(0);
  const [typeId, setTypeId] = useState(0);

  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const columnWidth = `${100 / columns - 4}%`;

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      setLoading(true);
      try {
        const [r, s, t, i] = await Promise.all([
          ApiService.getAllRegions(),
          ApiService.getSectors(),
          ApiService.getAllInvestmentTypes(),
          ApiService.getInvestInstruments(),
        ]);
        setRegions(r);
        setSectors(s);
        setTypes(t);
        setAllItems(i);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const items = useMemo(() => {
    let data = allItems;
    if (regionId > 0) data = data.filter((i) => i.regionId === regionId);
    if (sectorId > 0) data = data.filter((i) => i.sectorId === sectorId);
    if (typeId > 0) data = data.filter((i) => i.investmentTypeId === typeId);
    return data;
  }, [regionId, sectorId, typeId, allItems]);

  const handleDelete = (id: number) => {
    Alert.alert("Confirm delete", "Delete this instrument?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          await ApiService.deleteInvestInstrument(id);
          setAllItems((p) => p.filter((x) => x.id !== id));
        },
      },
    ]);
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const renderPicker = (
    label: string,
    value: number,
    setValue: (v: number) => void,
    data: any[],
    labelKey = "name",
  ) => (
    <View style={[spacing.m2, { width: columnWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>{label}</Text>
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
            selectedValue={value}
            onValueChange={(v) => setValue(Number(v))}
            style={[
              globalStyles.pickerText,
              globalStyles.pickerWeb,
              { flex: 1 },
            ]}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="All" value={0} />
            {data.map((x) => (
              <Picker.Item key={x.id} label={x[labelKey]} value={x.id} />
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
      <Text style={[globalStyles.header, spacing.mb4]}>
        Investment Instruments
      </Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_INVEST_INSTRUMENT)}
      >
        <Text style={globalStyles.buttonText}>Add new instrument</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {renderPicker("Region", regionId, setRegionId, regions)}
        {renderPicker("Sector", sectorId, setSectorId, sectors)}
        {renderPicker("Type", typeId, setTypeId, types, "typeName")}
      </View>

      <View
        style={[
          globalStyles.row,
          spacing.mt4,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {items.map((i) => (
          <View
            key={i.id}
            style={[
              globalStyles.card,
              spacing.m2,
              { width: columnWidth, minWidth: 280 },
            ]}
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

            <View style={spacing.mt3}>
              <TouchableOpacity
                style={globalStyles.button}
                onPress={async () => {
                  const instrument = await ApiService.getInvestInstrumentById(
                    i.id,
                  );

                  router.push({
                    pathname: instrument.financialMetricId
                      ? ROUTES.EDIT_FINANCIAL_METRIC
                      : ROUTES.ADD_FINANCIAL_METRIC,
                    params: {
                      instrumentId: i.id,
                      metricId: instrument.financialMetricId ?? 0,
                    },
                  });
                }}
              >
                <Text style={globalStyles.buttonText}>
                  {i.financialMetricId
                    ? "Edit Financial Metric"
                    : "Add Financial Metric"}
                </Text>
              </TouchableOpacity>
            </View>

            <View style={spacing.mt2}>
              <TouchableOpacity
                style={globalStyles.button}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_FINANCIAL_REPORT_LIST,
                    params: { instrumentId: i.id },
                  })
                }
              >
                <Text style={globalStyles.buttonText}>Financial Reports</Text>
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
