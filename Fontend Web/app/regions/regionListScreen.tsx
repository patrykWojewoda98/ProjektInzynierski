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
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const RegionListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

  const [regions, setRegions] = useState<any[]>([]);
  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [regionCodes, setRegionCodes] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    employeeAuthGuard().then(setIsReady);
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const [r, rl, rc] = await Promise.all([
          ApiService.getAllRegions(),
          ApiService.getRiskLevels(),
          ApiService.getAllRegionCodes(),
        ]);

        setRegions(r);
        setRiskLevels(rl);
        setRegionCodes(rc);
      } catch {
        Alert.alert("Error", "Failed to load regions.");
      } finally {
        setLoading(false);
      }
    };

    load();
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
          setRegions((p) => p.filter((r) => r.id !== id));
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

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_REGION)}
      >
        <Text style={globalStyles.buttonText}>Add new Region</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {regions.map((region) => (
          <View key={region.id} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
              <Text style={globalStyles.cardTitle}>{region.name}</Text>

              <Text style={globalStyles.textSmall}>
                Risk level: {getRiskDescription(region.regionRiskLevelId)}
              </Text>

              <Text style={globalStyles.textSmall}>
                Region code: {getRegionCode(region.regionCodeId)}
              </Text>

              <View
                style={[
                  globalStyles.row,
                  spacing.mt2,
                  { justifyContent: "center" },
                ]}
              >
                <TouchableOpacity
                  style={spacing.mr4}
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
      </View>

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
