import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../../assets/Constants/colors";
import { globalStyles, spacing } from "../../../assets/styles/styles";
import { ROUTES } from "../../../routes";
import ApiService from "../../../services/api";
import { confirmAction } from "../../../utils/confirmAction";
import { employeeAuthGuard } from "../../../utils/employeeAuthGuard";
import { useResponsiveColumns } from "../../../utils/useResponsiveColumns";
import { AuthContext } from "../../_layout";

const EmployeeListScreen = () => {
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns(4);

  const { user } = useContext(AuthContext);
  const isAdmin = Boolean(user?.isAdmin);

  const [employees, setEmployees] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      setIsReady(!!ok);
      if (!ok) setLoading(false);
    };
    check();
  }, []);

  useEffect(() => {
    if (!isReady) return;

    ApiService.getAllEmployees()
      .then(setEmployees)
      .catch(() => Alert.alert("Error", "Failed to load employees."))
      .finally(() => setLoading(false));
  }, [isReady]);

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this employee?",
      onConfirm: async () => {
        try {
          await ApiService.deleteEmployee(id);
          setEmployees((p) => p.filter((e) => e.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete employee.");
        }
      },
    });
  };

  if (!isReady || loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Employees</Text>

      {isAdmin && (
        <TouchableOpacity
          style={[globalStyles.button, spacing.mb4]}
          onPress={() => router.push(ROUTES.ADD_EMPLOYEE)}
        >
          <Text style={globalStyles.buttonText}>Add new employee</Text>
        </TouchableOpacity>
      )}

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {employees.map((e) => (
          <View key={e.id} style={[spacing.m2, { width: itemWidth }]}>
            <View style={globalStyles.card}>
              <View style={[globalStyles.row, globalStyles.spaceBetween]}>
                <View>
                  <Text style={globalStyles.cardTitle}>{e.name}</Text>
                  <Text style={globalStyles.textSmall}>{e.email}</Text>
                  <Text style={globalStyles.textSmall}>
                    {e.isAdmin ? "Admin" : "Employee"}
                  </Text>
                </View>

                {isAdmin && (
                  <View style={globalStyles.row}>
                    <TouchableOpacity
                      style={spacing.mr3}
                      onPress={() =>
                        router.push({
                          pathname: ROUTES.EDIT_EMPLOYEE,
                          params: { id: e.id },
                        })
                      }
                    >
                      <Ionicons
                        name="pencil"
                        size={22}
                        color={COLORS.primary}
                      />
                    </TouchableOpacity>

                    <TouchableOpacity onPress={() => handleDelete(e.id)}>
                      <Ionicons name="trash" size={22} color={COLORS.error} />
                    </TouchableOpacity>
                  </View>
                )}
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

export default EmployeeListScreen;
