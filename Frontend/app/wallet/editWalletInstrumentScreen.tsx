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

const EditWalletInstrumentScreen = () => {
  const router = useRouter();
  const { id } = useLocalSearchParams<{ id: string }>();

  const walletInstrumentId = Number(id);

  const [walletId, setWalletId] = useState(0);
  const [investInstrumentId, setInvestInstrumentId] = useState(0);
  const [instrumentName, setInstrumentName] = useState("");
  const [quantity, setQuantity] = useState(0);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    const load = async () => {
      try {
        // 🔹 wallet instrument
        const walletInstrument =
          await ApiService.getWalletInstrumentById(walletInstrumentId);

        setWalletId(walletInstrument.walletId);
        setInvestInstrumentId(walletInstrument.investInstrumentId);
        setQuantity(walletInstrument.quantity);

        // 🔹 instrument name
        const instruments = await ApiService.getInvestInstruments();
        const found = instruments.find(
          (i: any) => i.id === walletInstrument.investInstrumentId,
        );

        setInstrumentName(found?.name ?? "Unknown instrument");
      } catch (err) {
        console.error(err);
        Alert.alert("Error", "Failed to load wallet instrument");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [walletInstrumentId]);

  const handleSave = async () => {
    setSaving(true);
    try {
      await ApiService.updateWalletInstrument(walletInstrumentId, {
        id: walletInstrumentId,
        walletId,
        investInstrumentId,
        quantity,
      });

      Alert.alert("Success", "Investment updated");
      router.back();
    } catch (err) {
      console.error(err);
      Alert.alert("Error", "Failed to update investment");
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      {/* TITLE */}
      <Text style={[globalStyles.header, spacing.mb4]}>{instrumentName}</Text>

      {/* QUANTITY */}
      <View style={spacing.mb4}>
        <Text style={globalStyles.label}>Quantity</Text>
        <TextInput
          style={globalStyles.input}
          keyboardType="numeric"
          value={quantity.toString()}
          onChangeText={(v) => setQuantity(Number(v) || 0)}
        />
      </View>

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
        <Text style={globalStyles.buttonText}>
          {saving ? "Saving..." : "Save changes"}
        </Text>
      </TouchableOpacity>

      {/* CANCEL */}
      <TouchableOpacity style={spacing.mt4} onPress={() => router.back()}>
        <Text style={globalStyles.link}>Cancel</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default EditWalletInstrumentScreen;
