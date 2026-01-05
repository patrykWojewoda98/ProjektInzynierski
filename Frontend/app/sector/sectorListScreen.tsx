import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const SectorListScreen = () => {
  const router = useRouter();
  const [sectors, setSectors] = useState<any[]>([]);
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
        const data = await ApiService.getSectors();
        setSectors(data);
      } catch {
        Alert.alert("Error", "Failed to load sectors.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleDelete = (id: number) => {
    Alert.alert("Confirm delete", "Delete this sector?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          try {
            await ApiService.deleteSector(id);
            setSectors((p) => p.filter((s) => s.id !== id));
          } catch {
            Alert.alert("Error", "Failed to delete sector.");
          }
        },
      },
    ]);
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Sectors</Text>

      {/* ➕ ADD */}
      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_SECTOR)}
      >
        <Text style={globalStyles.buttonText}>Add new sector</Text>
      </TouchableOpacity>

      {sectors.map((s) => (
        <View key={s.id} style={[globalStyles.cardWithLongText, spacing.mb3]}>
          <View style={[globalStyles.row, globalStyles.spaceBetween]}>
            <View style={{ flex: 1 }}>
              <Text style={globalStyles.cardTitle}>{s.name}</Text>
              <Text style={globalStyles.textLong}>
                {s.code} • {s.description}
              </Text>
            </View>

            <View style={globalStyles.row}>
              <TouchableOpacity
                style={spacing.mr3}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_SECTOR,
                    params: { id: s.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDelete(s.id)}>
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
            </View>
          </View>
        </View>
      ))}

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default SectorListScreen;
