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
import { globalStyles } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const CreateInvestProfile = () => {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<string[]>([]);
  const [formData, setFormData] = useState({
    profileName: "",
    acceptableRisk: "",
    investHorizon: "",
    targetReturn: "",
    maxDrawDown: "",
    clientId: "1",
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
    if (!formData.acceptableRisk) newErrors.push("Please select risk level");
    if (!formData.investHorizon)
      newErrors.push("Please select investment horizon");
    if (!formData.targetReturn) newErrors.push("Target return is required");
    if (!formData.maxDrawDown) newErrors.push("Max drawdown is required");

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
      Alert.alert("Success", "Investment profile created!");
      router.push("/mainMenu");
    } catch (error) {
      Alert.alert(
        "Error",
        Array.isArray(error)
          ? error.join("\n")
          : "Failed to create investment profile"
      );
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <ScrollView
      contentContainerStyle={globalStyles.centerContainer}
      keyboardShouldPersistTaps="handled"
    >
      <Image
        source={require("../../assets/images/Logo.png")}
        style={globalStyles.logo}
      />

      <Text style={globalStyles.header}>Create Investment Profile</Text>

      {/* Error messages */}
      {errors.length > 0 && (
        <View style={globalStyles.errorContainer}>
          {errors.map((error, index) => (
            <Text key={index} style={globalStyles.errorText}>
              {error}
            </Text>
          ))}
        </View>
      )}

      <View style={globalStyles.formContainer}>
        {/* Profile Name */}
        <TextInput
          style={globalStyles.input}
          placeholder="Profile Name"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.profileName}
          onChangeText={(text) => handleInputChange("profileName", text)}
        />

        {/* Acceptable Risk */}
        <Text style={globalStyles.label}>Acceptable Risk</Text>
        <View style={globalStyles.pickerContainer}>
          <Picker
            selectedValue={formData.acceptableRisk}
            onValueChange={(value) =>
              handleInputChange("acceptableRisk", value)
            }
            style={globalStyles.picker}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="Select risk level" value="" />
            <Picker.Item label="Very Low" value="VeryLow" />
            <Picker.Item label="Low" value="Low" />
            <Picker.Item label="Medium" value="Medium" />
            <Picker.Item label="High" value="High" />
            <Picker.Item label="Very High" value="VeryHigh" />
          </Picker>
        </View>

        {/* Investment Horizon */}
        <Text style={globalStyles.label}>Investment Horizon</Text>
        <View style={globalStyles.pickerContainer}>
          <Picker
            selectedValue={formData.investHorizon}
            onValueChange={(value) => handleInputChange("investHorizon", value)}
            style={globalStyles.picker}
            dropdownIconColor={COLORS.textGrey}
          >
            <Picker.Item label="Select horizon" value="" />
            <Picker.Item label="Short Term" value="ShortTerm" />
            <Picker.Item label="Medium Term" value="MediumTerm" />
            <Picker.Item label="Long Term" value="LongTerm" />
            <Picker.Item label="Very Long Term" value="VeryLongTerm" />
          </Picker>
        </View>

        {/* Target Return */}
        <TextInput
          style={globalStyles.input}
          placeholder="Target Return (%)"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="numeric"
          value={formData.targetReturn}
          onChangeText={(text) => handleInputChange("targetReturn", text)}
        />

        {/* Max Drawdown */}
        <TextInput
          style={globalStyles.input}
          placeholder="Max Drawdown (%)"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="numeric"
          value={formData.maxDrawDown}
          onChangeText={(text) => handleInputChange("maxDrawDown", text)}
        />

        {/* Submit Button */}
        <TouchableOpacity
          style={[
            globalStyles.button,
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
      </View>
    </ScrollView>
  );
};

export default CreateInvestProfile;
