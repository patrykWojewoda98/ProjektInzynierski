import * as FileSystem from "expo-file-system/legacy";
import { useRouter } from "expo-router";
import * as Sharing from "expo-sharing";
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
        // 1️⃣ get wallet (walletId)
        const wallet = await ApiService.getWalletByClientId(user.id);

        // 2️⃣ full investment summary
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

  // =========================
  // EXCEL: SHARE
  // =========================
  const handleShareExcel = async () => {
    try {
      if (!user?.id) return;

      setDownloadingExcel(true);

      const wallet = await ApiService.getWalletByClientId(user.id);
      const excelUri = await ApiService.downloadWalletExcel(wallet.id);

      await Sharing.shareAsync(excelUri, {
        mimeType:
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        dialogTitle: "Export Wallet to Excel",
        UTI: "com.microsoft.excel.xlsx",
      });
    } catch (error) {
      console.error("Excel share error:", error);
      Alert.alert("Error", "Failed to export wallet to Excel");
    } finally {
      setDownloadingExcel(false);
    }
  };

  // =========================
  // EXCEL: SAVE AS…
  // =========================
  const handleSaveExcelWithPicker = async () => {
    try {
      if (!user?.id) return;

      const wallet = await ApiService.getWalletByClientId(user.id);

      // 1️⃣ download to sandbox
      const tempUri = await ApiService.downloadWalletExcelToTemp(wallet.id);

      // 2️⃣ ask for directory
      const permissions =
        await FileSystem.StorageAccessFramework.requestDirectoryPermissionsAsync();

      if (!permissions.granted) return;

      // 3️⃣ create file
      const fileUri = await FileSystem.StorageAccessFramework.createFileAsync(
        permissions.directoryUri,
        `Wallet_${wallet.id}.xlsx`,
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      );

      // 4️⃣ copy content
      const base64 = await FileSystem.readAsStringAsync(tempUri, {
        encoding: FileSystem.EncodingType.Base64,
      });

      await FileSystem.writeAsStringAsync(fileUri, base64, {
        encoding: FileSystem.EncodingType.Base64,
      });

      Alert.alert("Saved", "Excel file saved successfully");
    } catch (error) {
      console.error("Excel save error:", error);
      Alert.alert("Error", "Failed to save Excel file");
    }
  };

  // =========================
  // RENDER
  // =========================
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
      <Text style={[globalStyles.header, spacing.mb4]}>My Walt</Text>

      {/* WALLET SUMMARY */}
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

      {/* EXPORT EXCEL */}
      <View style={spacing.mb5}>
        <TouchableOpacity
          style={[globalStyles.button, spacing.mb2]}
          onPress={handleShareExcel}
          disabled={downloadingExcel}
        >
          {downloadingExcel ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Export Wallet (Excel)</Text>
          )}
        </TouchableOpacity>

        <TouchableOpacity
          style={globalStyles.secondaryButton}
          onPress={handleSaveExcelWithPicker}
        >
          <Text style={globalStyles.secondaryButtonText}>Save Excel as…</Text>
        </TouchableOpacity>
      </View>

      {/* INVESTMENTS */}
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

      {/* ACTIONS */}
      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.py2,
          spacing.mt3,
        ]}
        onPress={() => router.push(ROUTES.INVEST_INSTRUMENT)}
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
        style={spacing.mt6}
        onPress={() => router.push(ROUTES.MAIN_MENU)}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default WalletScreen;
