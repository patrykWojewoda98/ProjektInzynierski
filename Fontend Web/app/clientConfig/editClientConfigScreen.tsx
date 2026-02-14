import * as DocumentPicker from "expo-document-picker";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useCallback, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Switch,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { employeeAuthGuard } from "../../utils/employeeAuthGuard";

const MENU_KEYS = [
  "InvestProfile",
  "WatchList",
  "Wallet",
  "MarketData",
  "InvestInstrument",
  "FinancialReport",
  "FinancialMetric",
  "MyAIAnalysisRequests",
  "CurrencyRateHistory",
  "GeneratePersonalReport",
];

const PLATFORM = "Web";

const EditClientConfigScreen = () => {
  const { id } = useLocalSearchParams<{ id: string }>();
  const router = useRouter();
  const [key, setKey] = useState("");
  const [displayText, setDisplayText] = useState("");
  const [description, setDescription] = useState("");
  const [imagePath, setImagePath] = useState("");
  const [orderIndex, setOrderIndex] = useState(0);
  const [isVisible, setIsVisible] = useState(true);
  const [isReady, setIsReady] = useState(false);
  const [saving, setSaving] = useState(false);
  const [uploading, setUploading] = useState(false);

  const load = useCallback(async () => {
    if (!id) return;
    try {
      const item = await ApiService.getEmployeeClientConfigById(Number(id));
      setKey(item.key ?? "");
      setDisplayText(item.displayText ?? "");
      setDescription(item.description ?? "");
      setImagePath(item.imagePath ?? "");
      setOrderIndex(item.orderIndex ?? 0);
      setIsVisible(item.isVisible ?? true);
    } catch {
      Alert.alert("Error", "Failed to load item.");
    }
  }, [id]);

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

  const pickImage = async () => {
    try {
      const result = await DocumentPicker.getDocumentAsync({
        type: "image/*",
        copyToCacheDirectory: true,
      });
      if (result.canceled) return;
      const asset = result.assets[0];
      // On web, expo-document-picker provides a real File; use it so the server receives file bytes.
      const fileToUpload =
        "file" in asset && asset.file != null
          ? asset.file
          : {
              uri: asset.uri,
              name: asset.name ?? "image.png",
              type: asset.mimeType ?? "image/png",
            };
      setUploading(true);
      const res = await ApiService.uploadEmployeeClientConfigImage(
        PLATFORM,
        fileToUpload as any,
      );
      if (res?.imagePath) setImagePath(res.imagePath);
    } catch {
      Alert.alert("Error", "Failed to upload image.");
    } finally {
      setUploading(false);
    }
  };

  const handleSave = async () => {
    if (!id || !key.trim() || !displayText.trim()) {
      Alert.alert("Validation", "Key and Display text are required.");
      return;
    }
    setSaving(true);
    try {
      await ApiService.updateEmployeeClientConfigItem(Number(id), {
        key: key.trim(),
        displayText: displayText.trim(),
        description: description.trim() || undefined,
        imagePath: imagePath.trim() || undefined,
        orderIndex,
        isVisible,
      });
      Alert.alert("Success", "Menu item updated.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update.");
    } finally {
      setSaving(false);
    }
  };

  if (!isReady) {
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
        Edit client menu item (Web)
      </Text>

      <View style={[globalStyles.card, spacing.m2]}>
        <Text style={globalStyles.label}>Display text</Text>
        <TextInput
          style={globalStyles.input}
          value={displayText}
          onChangeText={setDisplayText}
        />
      </View>

      <View style={[globalStyles.card, spacing.m2]}>
        <Text style={globalStyles.label}>Description</Text>
        <TextInput
          style={globalStyles.input}
          value={description}
          onChangeText={setDescription}
        />
      </View>

      <View style={[globalStyles.card, spacing.m2]}>
        <Text style={globalStyles.label}>Image path </Text>
        <TextInput
          style={globalStyles.input}
          value={imagePath}
          onChangeText={setImagePath}
        />
        <TouchableOpacity
          style={[globalStyles.button, spacing.mt2]}
          onPress={pickImage}
          disabled={uploading}
        >
          <Text style={globalStyles.buttonText}>
            {uploading ? "Uploading…" : "Pick & upload image"}
          </Text>
        </TouchableOpacity>
      </View>

      <View
        style={[
          globalStyles.card,
          spacing.m2,
          globalStyles.row,
          { justifyContent: "space-between", alignItems: "center" },
        ]}
      >
        <Text style={globalStyles.label}>Visible in client menu</Text>
        <Switch
          value={isVisible}
          onValueChange={setIsVisible}
          trackColor={{ false: "#666", true: COLORS.primary }}
        />
      </View>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mt4]}
        onPress={handleSave}
        disabled={saving}
      >
        <Text style={globalStyles.buttonText}>
          {saving ? "Saving…" : "Save"}
        </Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={[globalStyles.button, spacing.mt2]}
        onPress={() => router.back()}
      >
        <Text style={globalStyles.buttonText}>Cancel</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default EditClientConfigScreen;
