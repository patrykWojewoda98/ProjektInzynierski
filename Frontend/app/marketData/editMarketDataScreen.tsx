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
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const EditMarketDataScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [isReady, setIsReady] = useState(false);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const [date, setDate] = useState("");
  const [openPrice, setOpenPrice] = useState("");
  const [closePrice, setClosePrice] = useState("");
  const [highPrice, setHighPrice] = useState("");
  const [lowPrice, setLowPrice] = useState("");
  const [volume, setVolume] = useState("");

  // 🔐 AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  // 📥 LOAD MARKET DATA
  useEffect(() => {
    if (!id || !isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getMarketDataById(Number(id));

        setDate(data.date.split("T")[0]);
        setOpenPrice(String(data.openPrice));
        setClosePrice(String(data.closePrice));
        setHighPrice(String(data.highPrice));
        setLowPrice(String(data.lowPrice));
        setVolume(String(data.volume));
      } catch {
        Alert.alert("Error", "Failed to load market data.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id, isReady]);

  const handleSave = async () => {
    if (
      !date ||
      !openPrice ||
      !closePrice ||
      !highPrice ||
      !lowPrice ||
      !volume
    ) {
      Alert.alert("Validation error", "All fields are required.");
      return;
    }

    setSaving(true);

    try {
      await ApiService.updateMarketData(Number(id), {
        date,
        openPrice: Number(openPrice),
        closePrice: Number(closePrice),
        highPrice: Number(highPrice),
        lowPrice: Number(lowPrice),
        volume: Number(volume),
      });

      Alert.alert("Success", "Market data updated successfully.");
      router.replace(ROUTES.MARKET_DATA);
    } catch {
      Alert.alert("Error", "Failed to update market data.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Market Data</Text>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {/* DATE */}
        <Text style={globalStyles.label}>Date</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={date}
          onChangeText={setDate}
          placeholder="YYYY-MM-DD"
        />

        {/* OPEN */}
        <Text style={globalStyles.label}>Open Price</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={openPrice}
          onChangeText={setOpenPrice}
          keyboardType="numeric"
        />

        {/* CLOSE */}
        <Text style={globalStyles.label}>Close Price</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={closePrice}
          onChangeText={setClosePrice}
          keyboardType="numeric"
        />

        {/* HIGH */}
        <Text style={globalStyles.label}>High Price</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={highPrice}
          onChangeText={setHighPrice}
          keyboardType="numeric"
        />

        {/* LOW */}
        <Text style={globalStyles.label}>Low Price</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          value={lowPrice}
          onChangeText={setLowPrice}
          keyboardType="numeric"
        />

        {/* VOLUME */}
        <Text style={globalStyles.label}>Volume</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          value={volume}
          onChangeText={setVolume}
          keyboardType="numeric"
        />

        {/* SAVE */}
        <TouchableOpacity
          style={[
            globalStyles.button,
            globalStyles.fullWidth,
            saving && globalStyles.buttonDisabled,
          ]}
          disabled={saving}
          onPress={handleSave}
        >
          {saving ? (
            <ActivityIndicator color={COLORS.whiteHeader} />
          ) : (
            <Text style={globalStyles.buttonText}>Save changes</Text>
          )}
        </TouchableOpacity>
      </View>

      {/* ⬅ BACK */}
      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default EditMarketDataScreen;
