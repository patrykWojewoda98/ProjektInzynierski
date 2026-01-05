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
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";

const InvestmentTypeListScreen = () => {
  const router = useRouter();
  const [items, setItems] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [ready, setReady] = useState(false);

  useEffect(() => {
    employeeAuthGuard().then(setReady);
  }, []);

  useEffect(() => {
    if (!ready) return;

    ApiService.getAllInvestmentTypes()
      .then(setItems)
      .catch(() => Alert.alert("Error", "Failed to load investment types"))
      .finally(() => setLoading(false));
  }, [ready]);

  const handleDelete = (id: number) => {
    Alert.alert("Confirm", "Delete this investment type?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          await ApiService.deleteInvestmentType(id);
          setItems((p) => p.filter((x) => x.id !== id));
        },
      },
    ]);
  };

  if (!ready || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Investment Types</Text>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mb4]}
        onPress={() => router.push(ROUTES.ADD_INVESTMENT_TYPE)}
      >
        <Text style={globalStyles.buttonText}>Add new investment type</Text>
      </TouchableOpacity>

      {items.map((i) => (
        <View key={i.id} style={[globalStyles.card, spacing.mb3]}>
          <View style={[globalStyles.row, globalStyles.spaceBetween]}>
            <Text style={globalStyles.cardTitle}>{i.typeName}</Text>

            <View style={globalStyles.row}>
              <TouchableOpacity
                style={spacing.mr3}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_INVESTMENT_TYPE,
                    params: { id: i.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDelete(i.id)}>
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

export default InvestmentTypeListScreen;
