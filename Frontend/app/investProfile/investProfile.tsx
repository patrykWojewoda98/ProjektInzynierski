import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Platform,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";
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
  const { itemWidth } = useResponsiveColumns();

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

  useEffect(() => {
    ApiService.getRiskLevels()
      .then(setRiskLevels)
      .catch(() => Alert.alert("Error", "Failed to load risk levels"));

    ApiService.getInvestHorizons()
      .then(setInvestHorizons)
      .catch(() => Alert.alert("Error", "Failed to load investment horizons"));
  }, []);

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
      const payload = {
        profileName: formData.profileName,
        acceptableRiskLevelId: Number(formData.acceptableRiskLevelId),
        investHorizonId: Number(formData.investHorizonId),
        targetReturn: Number(formData.targetReturn),
        maxDrawDown: Number(formData.maxDrawDown),
        clientId: Number(formData.clientId),
      };

      if (isEditMode && profileId) {
        await ApiService.updateInvestProfile(profileId, {
          id: profileId,
          ...payload,
        });
        Alert.alert("Success", "Investment profile updated");
      } else {
        await ApiService.createInvestProfile(payload);
        Alert.alert("Success", "Investment profile created");
      }

      router.push({ pathname: ROUTES.MAIN_MENU });
    } catch {
      Alert.alert("Error", "Operation failed");
    } finally {
      setIsLoading(false);
    }
  };

  const renderPicker = (
    label: string,
    value: number | "",
    setValue: (v: any) => void,
    data: any[],
    labelKey = "name",
    format?: (x: any) => string,
  ) => (
    <View style={[spacing.m2, { width: itemWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>{label}</Text>

        <View
          style={[
            globalStyles.pickerWrapper,
            globalStyles.pickerWebWrapper,
            {
              flexDirection: "row",
              alignItems: "center",
              paddingHorizontal: 12,
              height: 48,
            },
          ]}
        >
          <Picker
            selectedValue={value}
            onValueChange={setValue}
            style={[
              globalStyles.pickerText,
              Platform.OS === "web" && globalStyles.pickerWeb,
              { flex: 1 },
            ]}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="Select" value="" />
            {data.map((x) => (
              <Picker.Item
                key={x.id}
                label={format ? format(x) : x[labelKey]}
                value={x.id}
              />
            ))}
          </Picker>
        </View>
      </View>
    </View>
  );

  const renderInput = (
    label: string,
    value: string,
    onChange: (v: string) => void,
    keyboardType: any = "default",
  ) => (
    <View style={[spacing.m2, { width: itemWidth }]}>
      <View style={globalStyles.card}>
        <Text style={globalStyles.label}>{label}</Text>
        <TextInput
          style={globalStyles.input}
          value={value}
          keyboardType={keyboardType}
          onChangeText={onChange}
        />
      </View>
    </View>
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
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

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {renderInput("Profile Name", formData.profileName, (v) =>
          handleChange("profileName", v),
        )}

        {renderPicker(
          "Risk Level",
          formData.acceptableRiskLevelId,
          (v) => handleChange("acceptableRiskLevelId", v),
          riskLevels,
          undefined,
          (r) => `Risk ${r.riskScale} – ${r.description}`,
        )}

        {renderPicker(
          "Investment Horizon",
          formData.investHorizonId,
          (v) => handleChange("investHorizonId", v),
          investHorizons,
          "horizon",
        )}

        {renderInput(
          "Target Return (%)",
          formData.targetReturn,
          (v) => handleChange("targetReturn", v),
          "decimal-pad",
        )}

        {renderInput(
          "Max Drawdown (%)",
          formData.maxDrawDown,
          (v) => handleChange("maxDrawDown", v),
          "decimal-pad",
        )}
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          spacing.mt4,
          isLoading && globalStyles.buttonDisabled,
        ]}
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
    </ScrollView>
  );
};

export default InvestProfile;
