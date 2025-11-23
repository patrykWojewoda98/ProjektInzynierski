import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useState } from "react";
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

type RiskLevel = "Low" | "Moderate" | "High" | "VeryHigh";
type InvestmentHorizon = "SHORT_TERM" | "MEDIUM_TERM" | "LONG_TERM";

const CreateInvestProfile = () => {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<string[]>([]);
  const [formData, setFormData] = useState({
    profileName: "",
    acceptableRisk: "" as RiskLevel | "",
    investHorizon: "" as InvestmentHorizon | "",
    targetReturn: "",
    maxDrawDown: "",
    clientId: "1", // Placeholder for future context
  });

  const handleInputChange = (field: string, value: string) => {
    if (field === "targetReturn" || field === "maxDrawDown") {
      value = value.replace(/[^0-9.]/g, "");
    }
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors.length > 0) setErrors([]);
  };

  const validateForm = (): boolean => {
    const newErrors: string[] = [];

    if (!formData.profileName.trim())
      newErrors.push("Profile name is required");
    if (!formData.acceptableRisk) newErrors.push("Please select a risk level");
    if (!formData.investHorizon)
      newErrors.push("Please select an investment horizon");
    if (!formData.targetReturn) newErrors.push("Target return is required");
    else if (
      isNaN(Number(formData.targetReturn)) ||
      Number(formData.targetReturn) <= 0
    )
      newErrors.push("Target return must be a positive number");

    if (!formData.maxDrawDown) newErrors.push("Maximum drawdown is required");
    else if (
      isNaN(Number(formData.maxDrawDown)) ||
      Number(formData.maxDrawDown) <= 0
    )
      newErrors.push("Maximum drawdown must be a positive number");

    setErrors(newErrors);
    return newErrors.length === 0;
  };

  const handleSubmit = async () => {
    if (!validateForm()) return;

    setIsLoading(true);
    try {
      const profileData = {
        ...formData,
        targetReturn: Number(formData.targetReturn),
        maxDrawDown: Number(formData.maxDrawDown),
      };

      await ApiService.createInvestProfile(profileData);
      Alert.alert("Success", "Investment profile created successfully!");
      router.push("/main-menu");
    } catch (error) {
      Alert.alert(
        "Error",
        Array.isArray(error) ? error[0] : "Failed to create investment profile"
      );
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <ScrollView
      contentContainerStyle={globalStyles.scrollContainer}
      keyboardShouldPersistTaps="handled"
    >
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, spacing.mb4]}
      />

      <Text style={[globalStyles.header, spacing.mb3]}>
        Create Investment Profile
      </Text>

      <Text
        style={[
          globalStyles.text,
          globalStyles.textCenter,
          spacing.mb6,
          globalStyles.pickerText,
        ]}
      >
        Tell us about your investment preferences
      </Text>

      {errors.length > 0 && (
        <View style={[globalStyles.errorContainer, spacing.mb4]}>
          {errors.map((error, index) => (
            <Text key={index} style={globalStyles.errorText}>
              {error}
            </Text>
          ))}
        </View>
      )}

      <View style={globalStyles.formContainer}>
        <Text style={globalStyles.label}>Profile Name</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          placeholder="e.g., My Investment Strategy"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.profileName}
          onChangeText={(text) => handleInputChange("profileName", text)}
        />

        <Text style={globalStyles.label}>Risk Tolerance</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb4]}>
          <Picker
            selectedValue={formData.acceptableRisk}
            onValueChange={(itemValue) =>
              handleInputChange("acceptableRisk", itemValue)
            }
            style={globalStyles.pickerText}
          >
            <Picker.Item label="Select risk tolerance" value="" />
            <Picker.Item label="Low" value="Low" />
            <Picker.Item label="Moderate" value="Moderate" />
            <Picker.Item label="High" value="High" />
            <Picker.Item label="Very High" value="VeryHigh" />
          </Picker>
        </View>

        <Text style={globalStyles.label}>Investment Horizon</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb4]}>
          <Picker
            selectedValue={formData.investHorizon}
            onValueChange={(itemValue) =>
              handleInputChange("investHorizon", itemValue)
            }
            style={globalStyles.pickerText}
          >
            <Picker.Item label="Select investment horizon" value="" />
            <Picker.Item label="Short Term (1–3 years)" value="SHORT_TERM" />
            <Picker.Item label="Medium Term (3–7 years)" value="MEDIUM_TERM" />
            <Picker.Item label="Long Term (7+ years)" value="LONG_TERM" />
          </Picker>
        </View>

        <Text style={globalStyles.label}>Target Return (%)</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb4]}
          placeholder="e.g., 8.5"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="decimal-pad"
          value={formData.targetReturn}
          onChangeText={(text) => handleInputChange("targetReturn", text)}
        />

        <Text style={globalStyles.label}>Maximum Drawdown (%)</Text>
        <TextInput
          style={[globalStyles.input, spacing.mb6]}
          placeholder="e.g., 15"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="decimal-pad"
          value={formData.maxDrawDown}
          onChangeText={(text) => handleInputChange("maxDrawDown", text)}
        />

        <TouchableOpacity
          style={[
            globalStyles.button,
            globalStyles.fullWidth,
            isLoading && globalStyles.buttonDisabled,
          ]}
          onPress={handleSubmit}
          disabled={isLoading}
        >
          {isLoading ? (
            <ActivityIndicator color={COLORS.whiteHeader} />
          ) : (
            <Text style={globalStyles.buttonText}>Create Profile</Text>
          )}
        </TouchableOpacity>

        <View style={[globalStyles.row, globalStyles.center, spacing.mt4]}>
          <Text style={[globalStyles.text, spacing.mr1]}>
            Want to skip for now?
          </Text>
          <TouchableOpacity onPress={() => router.push("/main-menu")}>
            <Text style={globalStyles.link}>Go to Dashboard</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

export default CreateInvestProfile;
