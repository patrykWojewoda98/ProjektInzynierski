import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Image,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { ROUTES } from "../../routes";

const InvestInstrument = () => {
  const router = useRouter();

  const [regions, setRegions] = useState([]);
  const [sectors, setSectors] = useState([]);
  const [types, setTypes] = useState([]);
  const [instruments, setInstruments] = useState([]);

  const [selectedRegion, setSelectedRegion] = useState<number | null>(null);
  const [selectedSector, setSelectedSector] = useState<number | null>(null);
  const [selectedType, setSelectedType] = useState<number | null>(null);

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadFilters = async () => {
      try {
        const [r, s, t] = await Promise.all([
          ApiService.getRegions(),
          ApiService.getSectors(),
          ApiService.getInvestInstruments(),
        ]);
        setRegions(r);
        setSectors(s);
        setTypes(t);
      } finally {
        setLoading(false);
      }
    };

    loadFilters();
  }, []);

  useEffect(() => {
    const loadInstruments = async () => {
      let data = [];

      if (selectedRegion) {
        data = await ApiService.getInvestInstrumentsByRegion(selectedRegion);
      } else if (selectedSector) {
        data = await ApiService.getInvestInstrumentsBySector(selectedSector);
      } else if (selectedType) {
        data = await ApiService.getInvestInstrumentsByType(selectedType);
      } else {
        data = await ApiService.getInvestInstruments();
      }

      setInstruments(data);
    };

    loadInstruments();
  }, [selectedRegion, selectedSector, selectedType]);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Investment Instruments
      </Text>

      {/* REGION */}
      <Text style={globalStyles.label}>Region</Text>
      <View style={globalStyles.pickerWrapper}>
        <Picker
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          selectedValue={selectedRegion}
          onValueChange={(v) => {
            setSelectedRegion(v);
            setSelectedSector(null);
            setSelectedType(null);
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
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          selectedValue={selectedSector}
          onValueChange={(v) => {
            setSelectedSector(v);
            setSelectedRegion(null);
            setSelectedType(null);
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
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
          selectedValue={selectedType}
          onValueChange={(v) => {
            setSelectedType(v);
            setSelectedRegion(null);
            setSelectedSector(null);
          }}
        >
          <Picker.Item label="All types" value={null} />
          {types.map((t) => (
            <Picker.Item key={t.id} label={t.typeName} value={t.id} />
          ))}
        </Picker>
      </View>

      {/* TABLE */}
      <View style={[spacing.mt3, globalStyles.fullWidth]}>
        {instruments.map((i) => (
          <View key={i.id} style={globalStyles.card}>
            <Text style={globalStyles.cardTitle}>Ticker: {i.ticker}</Text>
            <Text style={globalStyles.text}>Name: {i.name}</Text>
            <Text style={globalStyles.textSmall}>
              Description: {i.description}
            </Text>

            <TouchableOpacity
              style={[globalStyles.buttonSmall, spacing.mt2]}
              onPress={() =>
                router.push({
                  pathname: ROUTES.FINANCIAL_METRIC,
                  params: { id: i.id },
                })
              }
            >
              <Text style={globalStyles.buttonText}>See more</Text>
            </TouchableOpacity>
          </View>
        ))}
      </View>

      <TouchableOpacity
        style={[spacing.mt6]}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default InvestInstrument;
