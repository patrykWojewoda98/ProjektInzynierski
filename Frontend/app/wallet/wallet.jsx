import { useRouter } from "expo-router";
import { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { AuthContext } from "../_layout";

const WalletScreen = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);
  const [totalValue, setTotalValue] = useState(0);
  const [wallet, setWallet] = useState(null);
  const [currency, setCurrency] = useState(null);
  const [investmentSummary, setInvestmentSummary] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadWalletData = async () => {
      if (!user?.id) return;

      try {
        const walletResponse = await ApiService.getWalletByClientId(user.id);
        setWallet(walletResponse);

        const currencyResponse = await ApiService.getCurrencyByID(
          walletResponse.currencyId,
        );
        setCurrency(currencyResponse);

        const summary = await ApiService.getWalletInvestmentSummary(
          walletResponse.id,
        );
        setInvestmentSummary(summary);

        let investmentsValue = 0;

        for (const item of summary) {
          const marketData = await ApiService.getMarketDataByInstrumentId(
            item.instrumentId,
          );
          investmentsValue += item.totalQuantity * marketData.price;
        }

        setTotalValue(walletResponse.cashBalance + investmentsValue);
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
        <Text style={[globalStyles.text, spacing.mt1]}>
          Total account value:{" "}
          <Text style={{ color: COLORS.primary, fontWeight: "600" }}>
            {totalValue.toFixed(2)} {currency?.name}
          </Text>
        </Text>
      </View>

      {/* MY INVESTMENTS */}
      <Text style={[globalStyles.header, spacing.mb3]}>My Investments</Text>

      {investmentSummary.length === 0 ? (
        <Text style={globalStyles.text}>
          You don't have any investments yet.
        </Text>
      ) : (
        investmentSummary.map((item) => (
          <View
            key={item.instrumentId}
            style={[globalStyles.card, spacing.mb3]}
          >
            <Text style={globalStyles.cardTitle}>{item.instrumentName}</Text>

            <Text style={globalStyles.text}>
              Total quantity: {item.totalQuantity}
            </Text>
          </View>
        ))
      )}

      {/* NAVIGATION */}
      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.py2,
          spacing.mt1,
        ]}
        onPress={() =>
          router.push({
            pathname: ROUTES.INVEST_INSTRUMENT,
          })
        }
      >
        <Text style={globalStyles.buttonText}>Show all Invest Instruments</Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.py2,
          spacing.mt2,
        ]}
        onPress={() => router.push(ROUTES.WALLET_CHART)}
      >
        <Text style={globalStyles.buttonText}>Show Portfolio Chart</Text>
      </TouchableOpacity>

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
