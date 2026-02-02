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

const InvestmentTypeListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

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
    confirmAction({
      title: "Confirm action",
      message: "Are you sure you want to delete this investment type?",
      onConfirm: async () => {
        try {
          await ApiService.deleteInvestmentType(id);
          setItems((p) => p.filter((x) => x.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete investment type.");
        }
      },
    });
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

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {items.map((i) => (
          <View key={i.id} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
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

export default InvestmentTypeListScreen;
