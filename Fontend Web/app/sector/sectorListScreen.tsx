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

const SectorListScreen = () => {
  const router = useRouter();
  const { width } = useWindowDimensions();

  const getColumns = () => {
    if (width >= 1400) return 4;
    if (width >= 1100) return 3;
    if (width >= 700) return 2;
    return 1;
  };

  const columns = getColumns();
  const columnWidth = `${100 / columns - 4}%`;

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
    confirmAction({
      title: "Confirm delete",
      message: "Delete this sector?",
      onConfirm: async () => {
        try {
          await ApiService.deleteSector(id);
          setSectors((p) => p.filter((s) => s.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete sector.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Sectors</Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_SECTOR)}
      >
        <Text style={globalStyles.buttonText}>Add new sector</Text>
      </TouchableOpacity>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {sectors.map((s) => (
          <View key={s.id} style={[spacing.m2, { width: columnWidth }]}>
            <View style={globalStyles.cardWithLongText}>
              <Text style={globalStyles.cardTitle}>{s.name}</Text>
              <Text style={globalStyles.textLong}>
                {s.code} • {s.description}
              </Text>

              <View
                style={[
                  globalStyles.row,
                  spacing.mt3,
                  { justifyContent: "center" },
                ]}
              >
                <TouchableOpacity
                  style={spacing.mr4}
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

export default SectorListScreen;
