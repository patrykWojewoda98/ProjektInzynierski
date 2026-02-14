import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useCallback, useEffect, useState } from "react";
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

const PLATFORM = "Web";

type ClientConfigItem = {
  id: number;
  key: string;
  displayText: string;
  description?: string;
  imagePath?: string;
  orderIndex: number;
  isVisible: boolean;
};

const ClientConfigListScreen = () => {
  const router = useRouter();
  const [items, setItems] = useState<ClientConfigItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [isReady, setIsReady] = useState(false);

  const load = useCallback(async () => {
    try {
      const data = await ApiService.getEmployeeClientConfigList(PLATFORM);
      setItems(Array.isArray(data) ? data : []);
    } catch {
      Alert.alert("Error", "Failed to load client config.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    const check = async () => {
      const ok = await employeeAuthGuard();
      if (ok) {
        setIsReady(true);
        load();
      }
    };
    check();
  }, [load]);

  const handleDelete = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Remove this menu item from client interface?",
      onConfirm: async () => {
        try {
          await ApiService.deleteEmployeeClientConfigItem(id);
          setItems((p) => p.filter((i) => i.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete.");
        }
      },
    });
  };

  const handleToggleVisibility = async (id: number) => {
    try {
      const updated = await ApiService.toggleVisibilityEmployeeClientConfig(id);
      setItems((p) =>
        p.map((i) =>
          i.id === id ? { ...i, isVisible: updated.isVisible } : i,
        ),
      );
    } catch {
      Alert.alert("Error", "Failed to toggle visibility.");
    }
  };

  const moveItem = async (index: number, direction: "up" | "down") => {
    const newIndex = direction === "up" ? index - 1 : index + 1;
    if (newIndex < 0 || newIndex >= items.length) return;
    const reordered = [...items];
    const [removed] = reordered.splice(index, 1);
    reordered.splice(newIndex, 0, removed);
    const orderPayload = reordered.map((item, i) => ({
      id: item.id,
      orderIndex: i,
    }));
    try {
      await ApiService.reorderEmployeeClientConfig(orderPayload);
      setItems(reordered.map((item, i) => ({ ...item, orderIndex: i })));
    } catch {
      Alert.alert("Error", "Failed to reorder.");
    }
  };

  if (!isReady || loading) {
    return (
      <View style={globalStyles.centerContainer}>
        <ActivityIndicator size="large" color={COLORS.primary} />
      </View>
    );
  }

  return (
    <ScrollView
      contentContainerStyle={[globalStyles.scrollContainer, spacing.p4]}
    >
      <Text style={[globalStyles.header, spacing.mb4]}>
        Client interface (Web) – menu items
      </Text>
      <View style={spacing.mb4}>
        {items.map((item, index) => (
          <View
            key={item.id}
            style={[
              globalStyles.card,
              spacing.m2,
              { opacity: item.isVisible ? 1 : 0.6 },
            ]}
          >
            <View
              style={[
                globalStyles.row,
                { justifyContent: "space-between", flexWrap: "wrap" },
              ]}
            >
              <View style={{ flex: 1 }}>
                <Text style={globalStyles.cardTitle}>{item.displayText}</Text>
                <Text
                  style={[
                    globalStyles.text,
                    { fontSize: 12, color: COLORS.textGrey },
                  ]}
                >
                  Key: {item.key} • Order: {item.orderIndex} •{" "}
                  {item.isVisible ? "Visible" : "Hidden"}
                </Text>
              </View>
              <View
                style={[globalStyles.row, { alignItems: "center", gap: 8 }]}
              >
                <TouchableOpacity
                  onPress={() => moveItem(index, "up")}
                  disabled={index === 0}
                >
                  <Ionicons
                    name="chevron-up"
                    size={24}
                    color={index === 0 ? "#999" : COLORS.primary}
                  />
                </TouchableOpacity>
                <TouchableOpacity
                  onPress={() => moveItem(index, "down")}
                  disabled={index === items.length - 1}
                >
                  <Ionicons
                    name="chevron-down"
                    size={24}
                    color={index === items.length - 1 ? "#999" : COLORS.primary}
                  />
                </TouchableOpacity>
                <TouchableOpacity
                  onPress={() => handleToggleVisibility(item.id)}
                >
                  <Ionicons
                    name={item.isVisible ? "eye-off" : "eye"}
                    size={22}
                    color={COLORS.primary}
                  />
                </TouchableOpacity>
                <TouchableOpacity
                  onPress={() =>
                    router.push({
                      pathname: ROUTES.EDIT_CLIENT_CONFIG,
                      params: { id: String(item.id) },
                    })
                  }
                >
                  <Ionicons name="pencil" size={22} color={COLORS.primary} />
                </TouchableOpacity>
                <TouchableOpacity onPress={() => handleDelete(item.id)}>
                  <Ionicons name="trash" size={22} color={COLORS.error} />
                </TouchableOpacity>
              </View>
            </View>
          </View>
        ))}
      </View>

      {items.length === 0 && (
        <Text style={[globalStyles.text, spacing.mt4]}>
          No menu items. Add one to configure the client main menu.
        </Text>
      )}

      <TouchableOpacity
        style={[globalStyles.button, spacing.mt4]}
        onPress={() => router.back()}
      >
        <Text style={globalStyles.buttonText}>Back</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default ClientConfigListScreen;
