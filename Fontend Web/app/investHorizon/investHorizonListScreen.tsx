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

const InvestHorizonListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

  const [horizons, setHorizons] = useState<any[]>([]);
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

    ApiService.getInvestHorizons()
      .then(setHorizons)
      .catch(() => Alert.alert("Error", "Failed to load investment horizons."))
      .finally(() => setLoading(false));
  }, [isReady]);

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm action",
      message: "Are you sure you want to delete this investment horizon?",
      onConfirm: async () => {
        try {
          await ApiService.deleteInvestHorizon(id);
          setHorizons((p) => p.filter((h) => h.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete investment horizon.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        Investment Horizons
      </Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_INVEST_HORIZON)}
      >
        <Text style={globalStyles.buttonText}>Add new investment horizon</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {horizons.map((h) => (
          <View key={h.id} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
              <View style={[globalStyles.row, globalStyles.spaceBetween]}>
                <Text style={globalStyles.text}>{h.horizon}</Text>

                <View style={globalStyles.row}>
                  <TouchableOpacity
                    style={spacing.mr3}
                    onPress={() =>
                      router.push({
                        pathname: ROUTES.EDIT_INVEST_HORIZON,
                        params: { id: h.id },
                      })
                    }
                  >
                    <Ionicons name="pencil" size={22} color={COLORS.primary} />
                  </TouchableOpacity>

                  <TouchableOpacity onPress={() => handleDelete(h.id)}>
                    <Ionicons name="trash" size={22} color={COLORS.error} />
                  </TouchableOpacity>
                </View>
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

export default InvestHorizonListScreen;
