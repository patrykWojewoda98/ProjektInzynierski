import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { ROUTES } from "../../routes";
import { AuthContext } from "../_layout";

const InvestInstrument = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);

  const [regions, setRegions] = useState([]);
  const [sectors, setSectors] = useState([]);
  const [types, setTypes] = useState([]);
  const [instruments, setInstruments] = useState([]);

  const [selectedRegion, setSelectedRegion] = useState<number | null>(null);
  const [selectedSector, setSelectedSector] = useState<number | null>(null);
  const [selectedType, setSelectedType] = useState<number | null>(null);

  const [watchListItems, setWatchListItems] = useState<number[]>([]);
  const [loading, setLoading] = useState(true);

  const handleToggleWatchList = async (instrumentId: number) => {
    if (!user?.id) return;

    if (watchListItems.includes(instrumentId)) return;

    try {
      await ApiService.addWatchListItem(user.id, instrumentId);

      setWatchListItems((prev) => [...prev, instrumentId]);
    } catch (err) {
      console.error("Error adding to watchlist:", err);
    }
  };

  useEffect(() => {
    const loadWatchList = async () => {
      if (!user?.id) return;

      try {
        const items = await ApiService.getWatchListItemsByClientId(user.id);
        setWatchListItems(items.map((x) => x.investInstrumentId));
      } catch (err) {
        console.error("Error loading watchlist:", err);
      }
    };

    loadWatchList();
  }, [user]);

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

  // 🔹 LOAD INSTRUMENTS
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
          selectedValue={selectedRegion}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
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
          selectedValue={selectedSector}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
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
          selectedValue={selectedType}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
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

      {/* INSTRUMENTS */}
      <View style={[spacing.mt3, globalStyles.fullWidth]}>
        {instruments.map((i) => (
          <View
            key={i.id}
            style={[globalStyles.card, { position: "relative" }]}
          >
            <TouchableOpacity
              style={[globalStyles.star]}
              onPress={() => handleToggleWatchList(i.id)}
            >
              <Ionicons
                name={watchListItems.includes(i.id) ? "star" : "star-outline"}
                size={26}
                color="white"
              />
            </TouchableOpacity>

            <Text style={globalStyles.cardTitle}>Ticker: {i.ticker}</Text>
            <Text style={globalStyles.text}>Name: {i.name}</Text>
            <Text style={globalStyles.textSmall}>
              Description: {i.description}
            </Text>

            <TouchableOpacity
              style={[
                globalStyles.button,
                globalStyles.fullWidth,
                spacing.py2,
                spacing.mt1,
              ]}
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
