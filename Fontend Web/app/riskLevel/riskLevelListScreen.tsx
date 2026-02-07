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
  useWindowDimensions,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { confirmAction } from "../../utils/confirmAction";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const RiskLevelListScreen = () => {
  const router = useRouter();
  const { width } = useWindowDimensions();

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const itemWidth = `${100 / columns - 4}%`;

  const [riskLevels, setRiskLevels] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getRiskLevels();
        setRiskLevels(data);
      } catch {
        Alert.alert("Error", "Failed to load risk levels.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this risk level?",
      onConfirm: async () => {
        try {
          await ApiService.deleteRiskLevel(id);
          setRiskLevels((p) => p.filter((r) => r.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete risk level.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Risk Levels</Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_RISK_LEVEL)}
      >
        <Text style={globalStyles.buttonText}>Add new risk level</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {riskLevels.map((r) => (
          <View key={r.id} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
              <Text style={globalStyles.cardTitle}>Scale: {r.riskScale}</Text>
              <Text style={[globalStyles.textSmall, spacing.mb2]}>
                {r.description}
              </Text>

              <View
                style={[
                  globalStyles.row,
                  { justifyContent: "center" },
                  spacing.mt2,
                ]}
              >
                <TouchableOpacity
                  style={spacing.mr4}
                  onPress={() =>
                    router.push({
                      pathname: ROUTES.EDIT_RISK_LEVEL,
                      params: { id: r.id },
                    })
                  }
                >
                  <Ionicons name="pencil" size={22} color={COLORS.primary} />
                </TouchableOpacity>

                <TouchableOpacity onPress={() => handleDelete(r.id)}>
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

export default RiskLevelListScreen;
