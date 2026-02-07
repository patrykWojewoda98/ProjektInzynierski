import { useRouter } from "expo-router";
import { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
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

const WalletScreen = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);
  const { itemWidth } = useResponsiveColumns();

  const [walletSummary, setWalletSummary] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [downloadingExcel, setDownloadingExcel] = useState(false);

  useEffect(() => {
    const loadWalletSummary = async () => {
      if (!user?.id) return;

      try {
        const wallet = await ApiService.getWalletByClientId(user.id);
        const summary = await ApiService.getWalletInvestmentSummary(wallet.id);
        setWalletSummary(summary);
      } catch (err) {
        console.error("Wallet load error:", err);
        Alert.alert("Error", "Failed to load wallet data");
      } finally {
        setLoading(false);
      }
    };

    loadWalletSummary();
  }, [user]);

  const handleExportExcelWeb = async () => {
    if (!user?.id) return;

    try {
      const wallet = await ApiService.getWalletByClientId(user.id);
      ApiService.openWalletExcelExport(wallet.id);
    } catch (error) {
      console.error("Excel export error:", error);
      Alert.alert("Error", "Failed to export wallet to Excel");
    }
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  if (!walletSummary) {
    return (
      <View style={globalStyles.centerContainer}>
        <Text style={globalStyles.text}>Wallet not available</Text>
      </View>
    );
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>My Wallet</Text>

      <View style={[globalStyles.card, spacing.mb4, globalStyles.fullWidth]}>
        <Text style={globalStyles.cardTitle}>Wallet Summary</Text>

        <Text style={[globalStyles.text, spacing.mt1]}>
          Total account value:{" "}
          <Text style={{ color: COLORS.primary, fontWeight: "600" }}>
            {walletSummary.totalAccountValue.toFixed(2)}{" "}
            {walletSummary.accountCurrency}
          </Text>
        </Text>

        <Text style={[globalStyles.text, spacing.mt1]}>
          Cash balance:{" "}
          <Text style={{ fontWeight: "600" }}>
            {walletSummary.cashBalance.toFixed(2)}{" "}
            {walletSummary.accountCurrency}
          </Text>
        </Text>

        <TouchableOpacity
          style={[globalStyles.button, spacing.mt3]}
          onPress={() => router.push(ROUTES.EDIT_WALLET)}
        >
          <Text style={globalStyles.buttonText}>Edit Wallet</Text>
        </TouchableOpacity>
      </View>

      <View style={spacing.mb5}>
        <TouchableOpacity
          style={[globalStyles.button]}
          onPress={handleExportExcelWeb}
          disabled={downloadingExcel}
        >
          {downloadingExcel ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Export Wallet (Excel)</Text>
          )}
        </TouchableOpacity>
      </View>

      <Text style={[globalStyles.header, spacing.mb3]}>My Investments</Text>

      {walletSummary.investments.length === 0 ? (
        <Text style={globalStyles.text}>
          You don't have any investments yet.
        </Text>
      ) : (
        <View
          style={[
            globalStyles.row,
            { flexWrap: "wrap", justifyContent: "center", width: "100%" },
          ]}
        >
          {walletSummary.investments.map((item: any) => (
            <View
              key={item.instrumentId}
              style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
            >
              <Text style={globalStyles.cardTitle}>{item.instrumentName}</Text>

              <Text style={globalStyles.text}>
                Quantity:{" "}
                <Text style={{ fontWeight: "600" }}>{item.totalQuantity}</Text>
              </Text>

              <Text style={globalStyles.text}>
                Unit Price:{" "}
                <Text style={{ fontWeight: "600" }}>
                  {item.pricePerUnit.toFixed(2)} {item.instrumentCurrency}
                </Text>
              </Text>

              <Text style={globalStyles.text}>
                Value:{" "}
                <Text style={{ fontWeight: "600", color: COLORS.primary }}>
                  {item.valueInAccountCurrency.toFixed(2)}{" "}
                  {walletSummary.accountCurrency}
                </Text>
              </Text>
            </View>
          ))}
        </View>
      )}

      <TouchableOpacity
        style={[globalStyles.button, globalStyles.fullWidth, spacing.mt3]}
        onPress={() => router.push(ROUTES.INVEST_INSTRUMENT)}
      >
        <Text style={globalStyles.buttonText}>Show all Invest Instruments</Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={[globalStyles.button, globalStyles.fullWidth, spacing.mt2]}
        onPress={() => router.push(ROUTES.WALLET_CHART)}
      >
        <Text style={globalStyles.buttonText}>Show Portfolio Chart</Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={spacing.mt6}
        onPress={() => router.push(ROUTES.MAIN_MENU)}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default WalletScreen;
