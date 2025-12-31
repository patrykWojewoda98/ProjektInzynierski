import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  View,
  TouchableOpacity,
} from "react-native";
import { useRouter } from "expo-router";

import { AuthContext } from "../_layout";
import ApiService from "../../services/api";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";

const WalletScreen = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);

  const [wallet, setWallet] = useState(null);
  const [currency, setCurrency] = useState(null);
  const [walletInstruments, setWalletInstruments] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadWalletData = async () => {
      if (!user?.id) return;

      try {
        // 1️⃣ GET OR CREATE WALLET
        const walletResponse = await ApiService.getWalletByClientId(user.id);
        setWallet(walletResponse);

        // 2️⃣ GET CURRENCY
        const currencyResponse = await ApiService.getCurrencyByID(
          walletResponse.currencyId
        );
        setCurrency(currencyResponse);

        // 3️⃣ GET WALLET INSTRUMENTS
        const instruments = await ApiService.getWalletInstrumentsByWalletId(
          walletResponse.id
        );
        setWalletInstruments(instruments);
      } catch (error) {
        console.error("Wallet load error:", error);
      } finally {
        setLoading(false);
      }
    };

    loadWalletData();
  }, [user]);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  if (!wallet) {
    return (
      <View style={globalStyles.centerContainer}>
        <Text style={globalStyles.text}>Wallet not available</Text>
      </View>
    );
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>My Wallet</Text>

      {/* WALLET SUMMARY */}
      <View style={[globalStyles.card, spacing.mb4]}>
        <Text style={globalStyles.cardTitle}>Wallet Summary</Text>
        <Text style={globalStyles.text}>
          Cash: {wallet.cashBalance} {currency?.name ?? ""}
        </Text>
      </View>

      {/* WALLET INSTRUMENTS */}
      <Text style={[globalStyles.header, spacing.mb3]}>My Investments</Text>

      {walletInstruments.length === 0 ? (
        <Text style={globalStyles.text}>
          You don't have any investment instruments yet.
        </Text>
      ) : (
        walletInstruments.map((wi) => (
          <View key={wi.id} style={[globalStyles.card, spacing.mb3]}>
            <Text style={globalStyles.cardTitle}>
              Instrument ID: {wi.investInstrumentId}
            </Text>
            <Text style={globalStyles.text}>Quantity: {wi.quantity}</Text>
            <Text style={globalStyles.textSmall}>
              Bought at: {new Date(wi.createdAt).toLocaleDateString()}
            </Text>
          </View>
        ))
      )}

      {/* BACK BUTTON */}
      <TouchableOpacity
        style={[spacing.mt6]}
        onPress={() => router.push(ROUTES.MAIN_MENU)}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default WalletScreen;
