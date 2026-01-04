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

const CountryListScreen = () => {
  const router = useRouter();

  const [countries, setCountries] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  // 🔐 AUTH
  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) setIsReady(true);
    };
    check();
  }, []);

  // 📥 LOAD
  useEffect(() => {
    if (!isReady) return;

    const load = async () => {
      try {
        const data = await ApiService.getAllCountries();
        setCountries(data);
      } catch {
        Alert.alert("Error", "Failed to load countries.");
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [isReady]);

  const handleDelete = (id: number) => {
    Alert.alert("Confirm delete", "Delete this country?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          try {
            await ApiService.deleteCountry(id);
            setCountries((p) => p.filter((c) => c.id !== id));
          } catch {
            Alert.alert("Error", "Failed to delete country.");
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
      <Text style={[globalStyles.header, spacing.mb4]}>Countries</Text>

      {/* ➕ ADD */}
      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_COUNTRY)}
      >
        <Text style={globalStyles.buttonText}>Add new country</Text>
      </TouchableOpacity>

      {countries.map((c) => (
        <View key={c.id} style={[globalStyles.card, spacing.mb3]}>
          <View style={[globalStyles.row, globalStyles.spaceBetween]}>
            <View>
              <Text style={globalStyles.cardTitle}>{c.name}</Text>
              <Text style={globalStyles.textSmall}>ISO: {c.isoCode}</Text>
            </View>

            <View style={globalStyles.row}>
              <TouchableOpacity
                style={spacing.mr3}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_COUNTRY,
                    params: { id: c.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDelete(c.id)}>
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

export default CountryListScreen;
