import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useContext, useEffect, useState } from "react";
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
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";
import { AuthContext } from "../_layout";

const EditWalletScreen = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);
  const { itemWidth } = useResponsiveColumns();

  const [walletId, setWalletId] = useState<number | null>(null);
  const [cash, setCash] = useState(0);
  const [currency, setCurrency] = useState("");

  const [investments, setInvestments] = useState<any[]>([]);
  const [instrumentMap, setInstrumentMap] = useState<Record<number, string>>(
    {},
  );

  const [loading, setLoading] = useState(true);
  const [savingCash, setSavingCash] = useState(false);

  useEffect(() => {
    const load = async () => {
      if (!user?.id) return;

      try {
        const wallet = await ApiService.getWalletByClientId(user.id);
        setWalletId(wallet.id);

        // CASH
        const summary = await ApiService.getWalletInvestmentSummary(wallet.id);
        setCash(summary.cashBalance);
        setCurrency(summary.accountCurrency);

        // WALLET INSTRUMENTS
        const walletInstruments =
          await ApiService.getWalletInstrumentsByWalletId(wallet.id);
        setInvestments(walletInstruments);

        // ALL INVEST INSTRUMENTS (id -> name)
        const allInstruments = await ApiService.getInvestInstruments();
        const map: Record<number, string> = {};
        allInstruments.forEach((i: any) => {
          map[i.id] = i.name;
        });
        setInstrumentMap(map);
      } catch (err) {
        console.error(err);
        Alert.alert("Error", "Failed to load wallet");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [user]);

  const handleUpdateCash = async () => {
    if (!walletId) return;

    setSavingCash(true);
    try {
      await ApiService.updateWallet(walletId, { cashBalance: cash });
      Alert.alert("Success", "Cash balance updated");
    } catch {
      Alert.alert("Error", "Failed to update cash");
    } finally {
      setSavingCash(false);
    }
  };

  const handleDelete = (walletInstrumentId: number) => {
    Alert.alert(
      "Confirm delete",
      "Are you sure you want to remove this investment?",
      [
        { text: "Cancel", style: "cancel" },
        {
          text: "Delete",
          style: "destructive",
          onPress: async () => {
            try {
              await ApiService.deleteWalletInstrument(walletInstrumentId);
              setInvestments((prev) =>
                prev.filter((x) => x.id !== walletInstrumentId),
              );
            } catch {
              Alert.alert("Error", "Failed to delete investment");
            }
          },
        },
      ],
    );
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Wallet</Text>

      {/* CASH */}
      <View style={[globalStyles.card, spacing.mb3, globalStyles.fullWidth]}>
        <Text style={globalStyles.cardTitle}>Cash</Text>

        <View style={[globalStyles.row, spacing.mt2]}>
          <TextInput
            style={[globalStyles.input, { flex: 1 }]}
            keyboardType="numeric"
            value={cash.toString()}
            onChangeText={(v) => setCash(Number(v) || 0)}
          />
          <Text style={[globalStyles.text, spacing.ml2]}>{currency}</Text>
        </View>
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.mb5,
          savingCash && globalStyles.buttonDisabled,
        ]}
        disabled={savingCash}
        onPress={handleUpdateCash}
      >
        <Text style={globalStyles.buttonText}>
          {savingCash ? "Saving..." : "Update Cash Balance"}
        </Text>
      </TouchableOpacity>

      {/* INVESTMENTS */}
      <Text style={[globalStyles.header, spacing.mb3]}>My Investments</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {investments.map((i) => (
          <View
            key={i.id}
            style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
          >
            {/* HEADER */}
            <View
              style={[globalStyles.row, globalStyles.spaceBetween, spacing.mb2]}
            >
              <Text style={globalStyles.cardTitle}>
                {instrumentMap[i.investInstrumentId] ??
                  `Instrument #${i.investInstrumentId}`}
              </Text>

              <View style={globalStyles.row}>
                <TouchableOpacity
                  onPress={() =>
                    router.push({
                      pathname: ROUTES.EDIT_WALLET_INSTRUMENT,
                      params: { id: i.id },
                    })
                  }
                >
                  <Ionicons name="pencil" size={22} color={COLORS.primary} />
                </TouchableOpacity>

                <TouchableOpacity
                  style={spacing.ml3}
                  onPress={() => handleDelete(i.id)}
                >
                  <Ionicons name="trash" size={22} color={COLORS.error} />
                </TouchableOpacity>
              </View>
            </View>

            {/* DETAILS */}
            <Text style={globalStyles.text}>Liczba: {i.quantity}</Text>
            <Text style={[globalStyles.text, spacing.mt1]}>
              Kupiono: {new Date(i.createdAt).toLocaleDateString()}
            </Text>
          </View>
        ))}
      </View>

      <TouchableOpacity style={spacing.mt6} onPress={() => router.back()}>
        <Text style={globalStyles.link}>Go back</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default EditWalletScreen;
