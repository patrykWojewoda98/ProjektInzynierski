import { MaterialIcons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
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

const RegisterScreen = () => {
  const router = useRouter();
  const [regions, setRegions] = useState([]);
  const [countries, setCountries] = useState([]);
  const [selectedRegion, setSelectedRegion] = useState(null);
  const [selectedCountry, setSelectedCountry] = useState(null);
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    city: "",
    address: "",
    postalCode: "",
    password: "",
    confirmPassword: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState({
    regions: true,
    countries: false,
    submitting: false,
  });
  const [errors, setErrors] = useState<string[]>([]);

  // Fetch regions on component mount
  useEffect(() => {
    const fetchRegions = async () => {
      try {
        const data = await ApiService.getRegions();
        setRegions(data);
      } catch (error) {
        setErrors(["Failed to load regions. Please try again."]);
      } finally {
        setLoading((prev) => ({ ...prev, regions: false }));
      }
    };

    fetchRegions();
  }, []);

  // Fetch countries when region is selected
  useEffect(() => {
    if (selectedRegion) {
      const fetchCountries = async () => {
        setLoading((prev) => ({ ...prev, countries: true }));
        try {
          const data = await ApiService.getCountriesByRegion(selectedRegion);
          setCountries(data);
        } catch (error) {
          setErrors(["Failed to load countries. Please try again."]);
        } finally {
          setLoading((prev) => ({ ...prev, countries: false }));
        }
      };

      fetchCountries();
    } else {
      setCountries([]);
      setSelectedCountry(null);
    }
  }, [selectedRegion]);

  // Handle form input changes
  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    // Clear error for this field if it exists
    if (errors.length > 0) {
      setErrors([]);
    }
  };

  // Validate form
  const validateForm = (): boolean => {
    const newErrors: string[] = [];

    if (!selectedRegion) {
      newErrors.push("Please select a region");
    }
    if (!selectedCountry) {
      newErrors.push("Please select a country");
    }
    if (!formData.name.trim()) {
      newErrors.push("Full name is required");
    }
    if (!formData.email.trim()) {
      newErrors.push("Email is required");
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.push("Please enter a valid email");
    }
    if (!formData.city.trim()) {
      newErrors.push("City is required");
    }
    if (!formData.address.trim()) {
      newErrors.push("Address is required");
    }
    if (!formData.postalCode.trim()) {
      newErrors.push("Postal code is required");
    }
    if (!formData.password) {
      newErrors.push("Password is required");
    } else if (formData.password.length < 6) {
      newErrors.push("Password must be at least 6 characters long");
    }
    if (formData.password !== formData.confirmPassword) {
      newErrors.push("Passwords do not match");
    }

    setErrors(newErrors);
    return newErrors.length === 0;
  };

  // Handle form submission
  const handleSubmit = async () => {
    if (!validateForm()) return;

    setLoading((prev) => ({ ...prev, submitting: true }));

    try {
      const clientData = {
        name: formData.name,
        email: formData.email,
        city: formData.city,
        address: formData.address,
        postalCode: formData.postalCode,
        password: formData.password,
        countryId: selectedCountry,
      };

      await ApiService.registerClient(clientData);
      Alert.alert("Success", "Registration successful!");
      // Optionally navigate to login or home screen
      router.push("/auth/login");
    } catch (error) {
      if (Array.isArray(error)) {
        setErrors(error);
      } else {
        setErrors(["An error occurred during registration. Please try again."]);
      }
    } finally {
      setLoading((prev) => ({ ...prev, submitting: false }));
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

      <Text style={globalStyles.header}>Register</Text>

      {/* Error Messages */}
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
        {/* Region Picker */}
        <View style={globalStyles.section}>
          <Text style={globalStyles.label}>Region</Text>
          <View
            style={[
              globalStyles.pickerContainer,
              errors.some((e) => e.includes("region")) &&
                globalStyles.inputError,
            ]}
          >
            <Picker
              selectedValue={selectedRegion}
              onValueChange={(itemValue) => {
                setSelectedRegion(itemValue);
                setSelectedCountry(null);
              }}
              style={globalStyles.picker}
              dropdownIconColor={COLORS.textGrey}
            >
              <Picker.Item label="Select region" value={null} />
              {regions.map((region) => (
                <Picker.Item
                  key={region.id}
                  label={region.name}
                  value={region.id}
                />
              ))}
            </Picker>
          </View>
        </View>

        {/* Country Picker */}
        <View style={globalStyles.section}>
          <Text style={globalStyles.label}>Country</Text>
          <View
            style={[
              globalStyles.pickerContainer,
              errors.some((e) => e.includes("country")) &&
                globalStyles.inputError,
            ]}
          >
            <Picker
              selectedValue={selectedCountry}
              onValueChange={(itemValue) => setSelectedCountry(itemValue)}
              style={globalStyles.picker}
              dropdownIconColor={COLORS.textGrey}
              enabled={!!selectedRegion && !loading.countries}
            >
              <Picker.Item label="Select country" value={null} />
              {countries.map((country) => (
                <Picker.Item
                  key={country.id}
                  label={country.name}
                  value={country.id}
                />
              ))}
            </Picker>
            {loading.countries && (
              <ActivityIndicator
                style={globalStyles.loadingIndicator}
                color={COLORS.primary}
              />
            )}
          </View>
        </View>

        {/* Full Name */}
        <TextInput
          style={[
            globalStyles.input,
            errors.some((e) => e.includes("name")) && globalStyles.inputError,
          ]}
          placeholder="Full Name"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.name}
          onChangeText={(text) => handleInputChange("name", text)}
        />

        {/* Email */}
        <TextInput
          style={[
            globalStyles.input,
            errors.some((e) => e.includes("email")) && globalStyles.inputError,
          ]}
          placeholder="Email"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="email-address"
          autoCapitalize="none"
          value={formData.email}
          onChangeText={(text) => handleInputChange("email", text)}
        />

        {/* City */}
        <TextInput
          style={[
            globalStyles.input,
            errors.some((e) => e.includes("city")) && globalStyles.inputError,
          ]}
          placeholder="City"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.city}
          onChangeText={(text) => handleInputChange("city", text)}
        />

        {/* Address */}
        <TextInput
          style={[
            globalStyles.input,
            errors.some((e) => e.includes("address")) &&
              globalStyles.inputError,
          ]}
          placeholder="Address"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.address}
          onChangeText={(text) => handleInputChange("address", text)}
        />

        {/* Postal Code */}
        <TextInput
          style={[
            globalStyles.input,
            errors.some((e) => e.includes("postal code")) &&
              globalStyles.inputError,
          ]}
          placeholder="Postal Code"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="number-pad"
          value={formData.postalCode}
          onChangeText={(text) => handleInputChange("postalCode", text)}
        />

        {/* Password */}
        <View style={globalStyles.section}>
          <Text style={globalStyles.label}>Password</Text>
          <View
            style={[
              globalStyles.passwordContainer,
              errors.some((e) => e.includes("Password")) &&
                globalStyles.inputError,
            ]}
          >
            <TextInput
              style={[
                globalStyles.input,
                { marginBottom: 0, borderWidth: 0, paddingRight: 50 },
              ]}
              placeholder="Enter your password"
              placeholderTextColor={COLORS.placeholderGrey}
              secureTextEntry={!showPassword}
              value={formData.password}
              onChangeText={(text) => handleInputChange("password", text)}
            />
            <TouchableOpacity
              style={globalStyles.eyeIcon}
              onPress={() => setShowPassword(!showPassword)}
            >
              <MaterialIcons
                name={showPassword ? "visibility-off" : "visibility"}
                size={24}
                color={COLORS.placeholderGrey}
              />
            </TouchableOpacity>
          </View>
        </View>

        {/* Confirm Password */}
        <View style={globalStyles.section}>
          <Text style={globalStyles.label}>Confirm Password</Text>
          <View
            style={[
              globalStyles.passwordContainer,
              errors.some((e) => e.includes("match")) &&
                globalStyles.inputError,
            ]}
          >
            <TextInput
              style={[
                globalStyles.input,
                { marginBottom: 0, borderWidth: 0, paddingRight: 50 },
              ]}
              placeholder="Confirm your password"
              placeholderTextColor={COLORS.placeholderGrey}
              secureTextEntry={!showPassword}
              value={formData.confirmPassword}
              onChangeText={(text) =>
                handleInputChange("confirmPassword", text)
              }
            />
          </View>
        </View>

        {/* Submit Button */}
        <TouchableOpacity
          style={[
            globalStyles.button,
            loading.submitting && globalStyles.buttonDisabled,
          ]}
          onPress={handleSubmit}
          disabled={loading.submitting}
        >
          {loading.submitting ? (
            <ActivityIndicator color={COLORS.whiteHeader} />
          ) : (
            <Text style={globalStyles.buttonText}>Register</Text>
          )}
        </TouchableOpacity>

        {/* Login Link */}
        <View
          style={{
            flexDirection: "row",
            justifyContent: "center",
            marginTop: 10,
          }}
        >
          <Text style={{ color: COLORS.textGrey }}>
            Already have an account?{" "}
          </Text>
          <TouchableOpacity onPress={() => router.push("/auth/login")}>
            <Text style={globalStyles.buttonText}>Login</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

export default RegisterScreen;
