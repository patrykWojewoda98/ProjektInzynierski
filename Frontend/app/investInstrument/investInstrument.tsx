import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Alert,
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

  const [selectedRegion, setSelectedRegion] = useState(null);
  const [selectedSector, setSelectedSector] = useState(null);
  const [selectedType, setSelectedType] = useState(null);

  const [quantities, setQuantities] = useState({});
  const [watchListMap, setWatchListMap] = useState({});

  const [loading, setLoading] = useState(true);
  const [watchListLoaded, setWatchListLoaded] = useState(false);

  const setQuantity = (instrumentId, value) => {
    const qty = Math.max(1, Number(value) || 1);
    setQuantities((prev) => ({
      ...prev,
      [instrumentId]: qty,
    }));
  };

  const handleAddToWallet = async (instrumentId) => {
    if (!user?.id) return;

    try {
      const wallet = await ApiService.getWalletByClientId(user.id);
      const quantity = quantities[instrumentId] ?? 1;

      await ApiService.addWalletInstrument(wallet.id, instrumentId, quantity);
      Alert.alert("Info", "Instrument added to wallet successfully.");
    } catch (err) {
      console.error("Error adding to wallet:", err);
    }
  };

  const handleToggleWatchList = async (instrumentId) => {
    if (!user?.id) return;

    const instrumentKey = Number(instrumentId);
    const watchListItemId = watchListMap[instrumentKey];

    try {
      if (watchListItemId) {
        await ApiService.deleteWatchListItem(watchListItemId);

        setWatchListMap((prev) => {
          const copy = { ...prev };
          delete copy[instrumentKey];
          return copy;
        });
      } else {
        const created = await ApiService.addWatchListItem(
          user.id,
          instrumentKey
        );

        setWatchListMap((prev) => ({
          ...prev,
          [instrumentKey]: created.id,
        }));
      }
    } catch (err) {
      console.error("Error toggling watchlist:", err);
    }
  };

  useEffect(() => {
    const loadWatchList = async () => {
      if (!user?.id) {
        setWatchListLoaded(true);
        return;
      }

      try {
        const raw = await ApiService.getWatchListItemsByClientId(user.id);

        const arr = Array.isArray(raw) ? raw : [raw];

        const map = {};
        arr.forEach((x) => {
          map[Number(x.investInstrumentId)] = x.id;
        });

        setWatchListMap(map);
      } catch (err) {
        console.error("Error loading watchlist:", err);
        setWatchListMap({});
      } finally {
        setWatchListLoaded(true);
      }
    };

    loadWatchList();
  }, [user]);

  // 🔹 LOAD FILTERS
  useEffect(() => {
    const loadFilters = async () => {
      try {
        const [r, s, i, t] = await Promise.all([
          ApiService.getAllRegions(),
          ApiService.getSectors(),
          ApiService.getInvestInstruments(),
          ApiService.getAllInvestmentTypes(),
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

  if (loading || !watchListLoaded) {
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
              style={globalStyles.star}
              onPress={() => handleToggleWatchList(i.id)}
            >
              <Ionicons
                name={watchListMap[Number(i.id)] ? "star" : "star-outline"}
                size={26}
                color="white"
              />
            </TouchableOpacity>

            <Text style={globalStyles.cardTitle}>Ticker: {i.ticker}</Text>
            <Text style={globalStyles.text}>Name: {i.name}</Text>
            <Text style={globalStyles.textSmall}>
              Description: {i.description}
            </Text>
            {/* QUANTITY + ADD TO WALLET */}
            <View style={[spacing.mt2]}>
              {/* Quantity row */}
              <View
                style={{
                  flexDirection: "row",
                  alignItems: "center",
                  justifyContent: "space-between",
                  marginBottom: 8,
                }}
              >
                <Text style={globalStyles.text}>Quantity</Text>

                <View style={{ flexDirection: "row", alignItems: "center" }}>
                  {/* MINUS */}
                  <TouchableOpacity
                    onPress={() =>
                      setQuantity(i.id, (quantities[i.id] ?? 1) - 1)
                    }
                    style={{ padding: 6 }}
                  >
                    <Ionicons
                      name="remove-circle-outline"
                      size={26}
                      color="white"
                    />
                  </TouchableOpacity>

                  {/* VALUE */}
                  <Text
                    style={[
                      globalStyles.text,
                      { minWidth: 30, textAlign: "center" },
                    ]}
                  >
                    {quantities[i.id] ?? 1}
                  </Text>

                  {/* PLUS */}
                  <TouchableOpacity
                    onPress={() =>
                      setQuantity(i.id, (quantities[i.id] ?? 1) + 1)
                    }
                    style={{ padding: 6 }}
                  >
                    <Ionicons
                      name="add-circle-outline"
                      size={26}
                      color="white"
                    />
                  </TouchableOpacity>
                </View>
              </View>

              {/* ADD TO WALLET */}
              <TouchableOpacity
                style={[
                  globalStyles.button,
                  globalStyles.fullWidth,
                  spacing.py2,
                ]}
                onPress={() => handleAddToWallet(i.id)}
              >
                <Text style={globalStyles.buttonText}>Add to Wallet</Text>
              </TouchableOpacity>
            </View>

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
