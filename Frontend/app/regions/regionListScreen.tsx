import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
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
import { confirmAction } from "../../utils/confirmAction";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const RegionListScreen = () => {
  const router = useRouter();

  const [regions, setRegions] = useState<any[]>([]);
  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [regionCodes, setRegionCodes] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  // 🔐 AUTH
  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  // 📥 LOAD DATA
  useEffect(() => {
    if (!isReady) return;

    const loadData = async () => {
      try {
        const [regionsData, risks, codes] = await Promise.all([
          ApiService.getAllRegions(),
          ApiService.getRiskLevels(),
          ApiService.getAllRegionCodes(),
        ]);

        setRegions(regionsData);
        setRiskLevels(risks);
        setRegionCodes(codes);
      } catch {
        Alert.alert("Error", "Failed to load regions.");
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [isReady]);

  const getRiskDescription = (id: number) =>
    riskLevels.find((r) => r.id === id)?.description ?? "N/A";

  const getRegionCode = (id: number | null) =>
    regionCodes.find((c) => c.id === id)?.code ?? "—";

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this region?",
      onConfirm: async () => {
        try {
          await ApiService.deleteRegion(id);
          setRegions((prev) => prev.filter((r) => r.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete region.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Regions</Text>

      {/* ➕ ADD */}
      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_REGION)}
      >
        <Text style={globalStyles.buttonText}>Add new Region</Text>
      </TouchableOpacity>

      {/* 📄 LIST */}
      {regions.map((region) => (
        <View key={region.id} style={[globalStyles.card, spacing.mb3]}>
          <View style={[globalStyles.row, globalStyles.spaceBetween]}>
            <View>
              <Text style={globalStyles.cardTitle}>{region.name}</Text>

              <Text style={globalStyles.textSmall}>
                Risk level: {getRiskDescription(region.regionRiskLevelId)}
              </Text>

              <Text style={globalStyles.textSmall}>
                Region code: {getRegionCode(region.regionCodeId)}
              </Text>
            </View>

            <View style={globalStyles.row}>
              <TouchableOpacity
                style={spacing.mr3}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_REGION,
                    params: { id: region.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDelete(region.id)}>
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
            </View>
          </View>
        </View>
      ))}

      {/* ⬅️ BACK */}
      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default RegionListScreen;
