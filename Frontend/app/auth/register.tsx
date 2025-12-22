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
import { globalStyles, spacing } from "../../assets/styles/styles";
import ApiService from "../../services/api";
import { ROUTES } from "../../routes";

const RegisterScreen = () => {
  const router = useRouter();
  const [regions, setRegions] = useState([]);
  const [countries, setCountries] = useState([]);
  const [selectedRegion, setSelectedRegion] = useState<number | null>(null);
  const [selectedCountry, setSelectedCountry] = useState<number | null>(null);
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
      } catch {
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
        } catch {
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

  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors.length > 0) setErrors([]);
  };

  const validateForm = (): boolean => {
    const newErrors: string[] = [];

    if (!selectedRegion) newErrors.push("Please select a region");
    if (!selectedCountry) newErrors.push("Please select a country");
    if (!formData.name.trim()) newErrors.push("Full name is required");
    if (!formData.email.trim()) newErrors.push("Email is required");
    else if (!/\S+@\S+\.\S+/.test(formData.email))
      newErrors.push("Please enter a valid email");
    if (!formData.city.trim()) newErrors.push("City is required");
    if (!formData.address.trim()) newErrors.push("Address is required");
    if (!formData.postalCode.trim()) newErrors.push("Postal code is required");
    if (!formData.password) newErrors.push("Password is required");
    else if (formData.password.length < 6)
      newErrors.push("Password must be at least 6 characters long");
    if (formData.password !== formData.confirmPassword)
      newErrors.push("Passwords do not match");

    setErrors(newErrors);
    return newErrors.length === 0;
  };

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
      router.push({ pathname: ROUTES.LOGIN });
    } catch {
      setErrors(["An error occurred during registration. Please try again."]);
    } finally {
      setLoading((prev) => ({ ...prev, submitting: false }));
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

      <Text style={[globalStyles.header, spacing.mb4]}>Register</Text>

      {/* Error Messages */}
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
        {/* Region Picker */}
        <Text style={globalStyles.label}>Region</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
          <Picker
            selectedValue={selectedRegion}
            onValueChange={(itemValue) => {
              setSelectedRegion(itemValue);
              setSelectedCountry(null);
            }}
            style={globalStyles.pickerText}
            enabled={!loading.regions}
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
          {loading.regions && (
            <ActivityIndicator
              color={COLORS.primary}
              style={{ marginTop: 5 }}
            />
          )}
        </View>

        {/* Country Picker */}
        <Text style={globalStyles.label}>Country</Text>
        <View style={[globalStyles.pickerWrapper, spacing.mb3]}>
          <Picker
            selectedValue={selectedCountry}
            onValueChange={(itemValue) => setSelectedCountry(itemValue)}
            style={globalStyles.pickerText}
            enabled={selectedRegion !== null && !loading.countries}
          >
            <Picker.Item
              label={selectedRegion ? "Select country" : "Select region first"}
              value={null}
            />
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
              color={COLORS.primary}
              style={{ marginTop: 5 }}
            />
          )}
        </View>

        {/* Full Name */}
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Full Name"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.name}
          onChangeText={(text) => handleInputChange("name", text)}
        />

        {/* Email */}
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Email"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="email-address"
          autoCapitalize="none"
          value={formData.email}
          onChangeText={(text) => handleInputChange("email", text)}
        />

        {/* City */}
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="City"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.city}
          onChangeText={(text) => handleInputChange("city", text)}
        />

        {/* Address */}
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Address"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.address}
          onChangeText={(text) => handleInputChange("address", text)}
        />

        {/* Postal Code */}
        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Postal Code"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="number-pad"
          value={formData.postalCode}
          onChangeText={(text) => handleInputChange("postalCode", text)}
        />

        {/* Password */}
        <Text style={globalStyles.label}>Password</Text>
        <View style={[globalStyles.passwordContainer, spacing.mb3]}>
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

        {/* Confirm Password */}
        <Text style={globalStyles.label}>Confirm Password</Text>
        <View style={[globalStyles.passwordContainer, spacing.mb4]}>
          <TextInput
            style={[
              globalStyles.input,
              { marginBottom: 0, borderWidth: 0, paddingRight: 50 },
            ]}
            placeholder="Confirm your password"
            placeholderTextColor={COLORS.placeholderGrey}
            secureTextEntry={!showPassword}
            value={formData.confirmPassword}
            onChangeText={(text) => handleInputChange("confirmPassword", text)}
          />
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
        <View style={[globalStyles.row, globalStyles.center, spacing.mt4]}>
          <Text style={[globalStyles.text, spacing.mr1]}>
            Already have an account?
          </Text>
          <TouchableOpacity
            onPress={() => router.push({ pathname: ROUTES.LOGIN })}
          >
            <Text style={globalStyles.link}>Login</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

export default RegisterScreen;
