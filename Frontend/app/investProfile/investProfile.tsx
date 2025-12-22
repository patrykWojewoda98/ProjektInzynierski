import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Image,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { ROUTES } from "../../routes";
import { AuthContext } from "../_layout";

type RiskLevelDto = {
  id: number;
  riskScale: number;
  description: string;
};

type InvestHorizonDto = {
  id: number;
  horizon: string;
};

const InvestProfile = () => {
  const { user } = useContext(AuthContext);
  const router = useRouter();

  const [isLoading, setIsLoading] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [profileId, setProfileId] = useState<number | null>(null);
  const [errors, setErrors] = useState<string[]>([]);

  const [riskLevels, setRiskLevels] = useState<RiskLevelDto[]>([]);
  const [investHorizons, setInvestHorizons] = useState<InvestHorizonDto[]>([]);

  const [formData, setFormData] = useState({
    profileName: "",
    acceptableRiskLevelId: "" as number | "",
    investHorizonId: "" as number | "",
    targetReturn: "",
    maxDrawDown: "",
    clientId: null as number | null,
  });

  /* ===== LOAD DICTIONARIES ===== */

  useEffect(() => {
    ApiService.getRiskLevels()
      .then((d) => setRiskLevels(d))
      .catch(() => Alert.alert("Error", "Failed to load risk levels"));

    ApiService.getInvestHorizons()
      .then((d) => setInvestHorizons(d))
      .catch(() => Alert.alert("Error", "Failed to load investment horizons"));
  }, []);

  /* ===== LOAD PROFILE ===== */

  useEffect(() => {
    if (!user?.id) return;

    setFormData((p) => ({ ...p, clientId: user.id }));

    ApiService.getInvestProfileByClientId(user.id)
      .then((profile) => {
        if (!profile) {
          setIsEditMode(false);
          return;
        }

        setIsEditMode(true);
        setProfileId(profile.id);

        setFormData({
          profileName: profile.profileName,
          acceptableRiskLevelId: profile.acceptableRiskLevelId,
          investHorizonId: profile.investHorizonId,
          targetReturn: String(profile.targetReturn),
          maxDrawDown: String(profile.maxDrawDown),
          clientId: user.id,
        });
      })
      .catch(() => Alert.alert("Error", "Failed to load profile"));
  }, [user]);

  /* ===== HELPERS ===== */

  const handleChange = (field: string, value: any) => {
    if (field === "targetReturn" || field === "maxDrawDown") {
      value = value.replace(/[^0-9.]/g, "");
    }
    setFormData((p) => ({ ...p, [field]: value }));
    setErrors([]);
  };

  const validate = () => {
    const e: string[] = [];
    if (!formData.profileName) e.push("Profile name is required");
    if (!formData.acceptableRiskLevelId) e.push("Risk level is required");
    if (!formData.investHorizonId) e.push("Investment horizon is required");
    if (!formData.targetReturn) e.push("Target return is required");
    if (!formData.maxDrawDown) e.push("Max drawdown is required");

    setErrors(e);
    return e.length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;

    setIsLoading(true);

    try {
      if (isEditMode && profileId) {
        const updatePayload = {
          id: profileId,
          profileName: formData.profileName,
          acceptableRiskLevelId: Number(formData.acceptableRiskLevelId),
          investHorizonId: Number(formData.investHorizonId),
          targetReturn: Number(formData.targetReturn),
          maxDrawDown: Number(formData.maxDrawDown),
          clientId: Number(formData.clientId),
        };

        await ApiService.updateInvestProfile(profileId, updatePayload);
        Alert.alert("Success", "Investment profile updated");
      } else {
        const createPayload = {
          profileName: formData.profileName,
          acceptableRiskLevelId: Number(formData.acceptableRiskLevelId),
          investHorizonId: Number(formData.investHorizonId),
          targetReturn: Number(formData.targetReturn),
          maxDrawDown: Number(formData.maxDrawDown),
          clientId: Number(formData.clientId),
        };

        await ApiService.createInvestProfile(createPayload);
        Alert.alert("Success", "Investment profile created");
      }

      router.push({ pathname: ROUTES.MAIN_MENU });
    } catch (err) {
      Alert.alert("Error", "Operation failed");
    } finally {
      setIsLoading(false);
    }
  };

  /* ===== UI ===== */

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, spacing.mb4]}
      />

      <Text style={[globalStyles.header, spacing.mb3]}>
        {isEditMode ? "Update Investment Profile" : "Create Investment Profile"}
      </Text>

      {errors.length > 0 && (
        <View style={globalStyles.errorContainer}>
          {errors.map((e, i) => (
            <Text key={i} style={globalStyles.errorText}>
              {e}
            </Text>
          ))}
        </View>
      )}

      <View style={globalStyles.formContainer}>
        <Text style={globalStyles.label}>Profile Name</Text>
        <TextInput
          style={globalStyles.input}
          value={formData.profileName}
          onChangeText={(v) => handleChange("profileName", v)}
        />

        <Text style={globalStyles.label}>Risk Level</Text>
        <View style={globalStyles.pickerWrapper}>
          <Picker
            selectedValue={formData.acceptableRiskLevelId}
            onValueChange={(v) => handleChange("acceptableRiskLevelId", v)}
            style={globalStyles.pickerText}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="Select" value="" />
            {riskLevels.map((r) => (
              <Picker.Item
                key={r.id}
                label={`Risk ${r.riskScale} – ${r.description}`}
                value={r.id}
              />
            ))}
          </Picker>
        </View>

        <Text style={globalStyles.label}>Investment Horizon</Text>
        <View style={globalStyles.pickerWrapper}>
          <View style={globalStyles.pickerWrapper}>
            <Picker
              selectedValue={formData.investHorizonId}
              onValueChange={(v) => handleChange("investHorizonId", v)}
              style={globalStyles.pickerText}
              dropdownIconColor={COLORS.textGrey}
            >
              <Picker.Item label="Select" value="" />
              {investHorizons.map((h) => (
                <Picker.Item key={h.id} label={h.horizon} value={h.id} />
              ))}
            </Picker>
          </View>
        </View>

        <Text style={globalStyles.label}>Target Return (%)</Text>
        <TextInput
          style={globalStyles.input}
          keyboardType="decimal-pad"
          value={formData.targetReturn}
          onChangeText={(v) => handleChange("targetReturn", v)}
        />

        <Text style={globalStyles.label}>Max Drawdown (%)</Text>
        <TextInput
          style={globalStyles.input}
          keyboardType="decimal-pad"
          value={formData.maxDrawDown}
          onChangeText={(v) => handleChange("maxDrawDown", v)}
        />

        <TouchableOpacity
          style={globalStyles.button}
          onPress={handleSubmit}
          disabled={isLoading}
        >
          {isLoading ? (
            <ActivityIndicator color={COLORS.whiteHeader} />
          ) : (
            <Text style={globalStyles.buttonText}>
              {isEditMode ? "Update" : "Create"}
            </Text>
          )}
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default InvestProfile;
