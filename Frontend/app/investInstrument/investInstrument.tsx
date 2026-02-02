import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useContext, useEffect, useMemo, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Platform,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";
import { AuthContext } from "../_layout";

const InvestInstrument = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);
  const { itemWidth } = useResponsiveColumns();

  const [regions, setRegions] = useState<any[]>([]);
  const [sectors, setSectors] = useState<any[]>([]);
  const [types, setTypes] = useState<any[]>([]);
  const [allInstruments, setAllInstruments] = useState<any[]>([]);

  const [selectedRegion, setSelectedRegion] = useState(0);
  const [selectedSector, setSelectedSector] = useState(0);
  const [selectedType, setSelectedType] = useState(0);

  const [quantities, setQuantities] = useState<Record<number, number>>({});
  const [watchListMap, setWatchListMap] = useState<Record<number, number>>({});

  const [loading, setLoading] = useState(true);
  const [watchListLoaded, setWatchListLoaded] = useState(false);

  const setQuantity = (instrumentId: number, value: number) => {
    const qty = Math.max(1, Number(value) || 1);
    setQuantities((prev) => ({ ...prev, [instrumentId]: qty }));
  };

  const handleAddToWallet = async (instrumentId: number) => {
    if (!user?.id) return;

    try {
      const wallet = await ApiService.getWalletByClientId(user.id);
      const quantity = quantities[instrumentId] ?? 1;
      await ApiService.addWalletInstrument(wallet.id, instrumentId, quantity);
      Alert.alert("Info", "Instrument added to wallet successfully.");
    } catch {}
  };

  const handleToggleWatchList = async (instrumentId: number) => {
    if (!user?.id) return;

    const watchListItemId = watchListMap[instrumentId];

    try {
      if (watchListItemId) {
        await ApiService.deleteWatchListItem(watchListItemId);
        setWatchListMap((prev) => {
          const copy = { ...prev };
          delete copy[instrumentId];
          return copy;
        });
      } else {
        const created = await ApiService.addWatchListItem(
          user.id,
          instrumentId,
        );
        setWatchListMap((prev) => ({ ...prev, [instrumentId]: created.id }));
      }
    } catch {}
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
        const map: Record<number, number> = {};
        arr.forEach((x) => (map[Number(x.investInstrumentId)] = x.id));
        setWatchListMap(map);
      } catch {
        setWatchListMap({});
      } finally {
        setWatchListLoaded(true);
      }
    };

    loadWatchList();
  }, [user]);

  useEffect(() => {
    const load = async () => {
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
        setAllInstruments(i);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, []);

  const filteredInstruments = useMemo(() => {
    let data = allInstruments;

    if (selectedRegion > 0)
      data = data.filter((i) => i.regionId === selectedRegion);
    if (selectedSector > 0)
      data = data.filter((i) => i.sectorId === selectedSector);
    if (selectedType > 0)
      data = data.filter((i) => i.investmentTypeId === selectedType);

    return data;
  }, [selectedRegion, selectedSector, selectedType, allInstruments]);

  if (loading || !watchListLoaded) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  const renderPicker = (
    label: string,
    value: number,
    setValue: (v: number) => void,
    data: any[],
    labelKey = "name",
  ) => (
    <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
      <Text style={globalStyles.label}>{label}</Text>

      <View style={[globalStyles.pickerWrapper, globalStyles.pickerWebWrapper]}>
        <Picker
          selectedValue={value}
          onValueChange={(v) => setValue(Number(v))}
          style={[
            globalStyles.pickerText,
            Platform.OS === "web" && globalStyles.pickerWeb,
          ]}
        >
          <Picker.Item label="All" value={0} />
          {data.map((x) => (
            <Picker.Item key={x.id} label={x[labelKey]} value={x.id} />
          ))}
        </Picker>

        {Platform.OS === "web" && (
          <Ionicons
            name="chevron-down"
            size={20}
            color={COLORS.textGrey}
            style={globalStyles.pickerWebArrow}
          />
        )}
      </View>
    </View>
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Investment Instruments
      </Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {renderPicker("Region", selectedRegion, setSelectedRegion, regions)}
        {renderPicker("Sector", selectedSector, setSelectedSector, sectors)}
        {renderPicker("Type", selectedType, setSelectedType, types, "typeName")}
      </View>

      <View
        style={[
          globalStyles.row,
          spacing.mt3,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {filteredInstruments.map((i) => (
          <View
            key={i.id}
            style={[
              globalStyles.card,
              spacing.m2,
              { width: itemWidth, position: "relative" },
            ]}
          >
            <TouchableOpacity
              style={globalStyles.star}
              onPress={() => handleToggleWatchList(i.id)}
            >
              <Ionicons
                name={watchListMap[i.id] ? "star" : "star-outline"}
                size={26}
                color="white"
              />
            </TouchableOpacity>

            <Text style={globalStyles.cardTitle}>Ticker: {i.ticker}</Text>
            <Text style={globalStyles.text}>Name: {i.name}</Text>
            <Text style={globalStyles.textSmall}>{i.description}</Text>

            <View style={spacing.mt2}>
              <View
                style={[
                  globalStyles.row,
                  globalStyles.spaceBetween,
                  spacing.mb2,
                ]}
              >
                <Text style={globalStyles.text}>Quantity</Text>

                <View style={globalStyles.row}>
                  <TouchableOpacity
                    onPress={() =>
                      setQuantity(i.id, (quantities[i.id] ?? 1) - 1)
                    }
                  >
                    <Ionicons
                      name="remove-circle-outline"
                      size={26}
                      color="white"
                    />
                  </TouchableOpacity>

                  <Text
                    style={[
                      globalStyles.text,
                      { minWidth: 30, textAlign: "center" },
                    ]}
                  >
                    {quantities[i.id] ?? 1}
                  </Text>

                  <TouchableOpacity
                    onPress={() =>
                      setQuantity(i.id, (quantities[i.id] ?? 1) + 1)
                    }
                  >
                    <Ionicons
                      name="add-circle-outline"
                      size={26}
                      color="white"
                    />
                  </TouchableOpacity>
                </View>
              </View>

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
          </View>
        ))}
      </View>

      <TouchableOpacity
        style={spacing.mt6}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default InvestInstrument;
