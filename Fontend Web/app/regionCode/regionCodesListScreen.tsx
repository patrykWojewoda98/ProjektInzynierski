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

const RegionCodesListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns();

  const [regionCodes, setRegionCodes] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    checkAuth();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    const loadData = async () => {
      try {
        const data = await ApiService.getAllRegionCodes();
        setRegionCodes(data);
      } catch {
        Alert.alert("Error", "Failed to load region codes.");
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [isReady]);

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Are you sure you want to delete this region code?",
      onConfirm: async () => {
        try {
          await ApiService.deleteRegionCode(id);
          setRegionCodes((prev) => prev.filter((r) => r.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete region code.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb3]}>Region Codes</Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_REGION_CODE)}
      >
        <Text style={globalStyles.buttonText}>Add new Region Code</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {regionCodes.map((code) => (
          <View
            key={code.id}
            style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
          >
            <View style={[globalStyles.row, globalStyles.spaceBetween]}>
              <Text style={globalStyles.cardTitle}>{code.code}</Text>

              <View style={globalStyles.row}>
                <TouchableOpacity
                  style={spacing.mr3}
                  onPress={() =>
                    router.push({
                      pathname: ROUTES.EDIT_REGION_CODE,
                      params: { id: code.id },
                    })
                  }
                >
                  <Ionicons name="pencil" size={22} color={COLORS.primary} />
                </TouchableOpacity>

                <TouchableOpacity onPress={() => handleDelete(code.id)}>
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

export default RegionCodesListScreen;
