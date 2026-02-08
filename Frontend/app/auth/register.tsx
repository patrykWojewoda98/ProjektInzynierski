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
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";

const RegisterScreen = () => {
  const router = useRouter();
  const [regions, setRegions] = useState<any[]>([]);
  const [countries, setCountries] = useState<any[]>([]);
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

  useEffect(() => {
    const fetchRegions = async () => {
      try {
        const data = await ApiService.getAllRegions();
        setRegions(data);
      } catch {
        setErrors(["Failed to load regions. Please try again."]);
      } finally {
        setLoading((p) => ({ ...p, regions: false }));
      }
    };
    fetchRegions();
  }, []);

  useEffect(() => {
    if (selectedRegion) {
      const fetchCountries = async () => {
        setLoading((p) => ({ ...p, countries: true }));
        try {
          const data = await ApiService.getCountriesByRegion(selectedRegion);
          setCountries(data);
        } catch {
          setErrors(["Failed to load countries. Please try again."]);
        } finally {
          setLoading((p) => ({ ...p, countries: false }));
        }
      };
      fetchCountries();
    } else {
      setCountries([]);
      setSelectedCountry(null);
    }
  }, [selectedRegion]);

  const handleInputChange = (field: string, value: string) => {
    setFormData((p) => ({ ...p, [field]: value }));
    if (errors.length > 0) setErrors([]);
  };

  const validateForm = () => {
    const e: string[] = [];
    if (!selectedRegion) e.push("Please select a region");
    if (!selectedCountry) e.push("Please select a country");
    if (!formData.name.trim()) e.push("Full name is required");
    if (!formData.email.trim()) e.push("Email is required");
    else if (!/\S+@\S+\.\S+/.test(formData.email))
      e.push("Please enter a valid email");
    if (!formData.city.trim()) e.push("City is required");
    if (!formData.address.trim()) e.push("Address is required");
    if (!formData.postalCode.trim()) e.push("Postal code is required");
    if (!formData.password) e.push("Password is required");
    else if (formData.password.length < 6)
      e.push("Password must be at least 6 characters long");
    if (formData.password !== formData.confirmPassword)
      e.push("Passwords do not match");
    setErrors(e);
    return e.length === 0;
  };

  const handleSubmit = async () => {
    if (!validateForm()) return;
    setLoading((p) => ({ ...p, submitting: true }));
    try {
      await ApiService.registerClient({
        name: formData.name,
        email: formData.email,
        city: formData.city,
        address: formData.address,
        postalCode: formData.postalCode,
        password: formData.password,
        countryId: selectedCountry,
      });
      Alert.alert("Success", "Registration successful!");
      router.push({ pathname: ROUTES.LOGIN });
    } catch {
      setErrors(["An error occurred during registration. Please try again."]);
    } finally {
      setLoading((p) => ({ ...p, submitting: false }));
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

      {errors.length > 0 && (
        <View style={[globalStyles.errorContainer, spacing.mb4]}>
          {errors.map((e, i) => (
            <Text key={i} style={globalStyles.errorText}>
              {e}
            </Text>
          ))}
        </View>
      )}

      <View style={globalStyles.formContainer}>
        <View style={spacing.mb3}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>Region</Text>
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
                selectedValue={selectedRegion}
                onValueChange={(v) => {
                  setSelectedRegion(v);
                  setSelectedCountry(null);
                }}
                style={[
                  globalStyles.pickerText,
                  globalStyles.pickerWeb,
                  { flex: 1 },
                ]}
                dropdownIconColor={COLORS.textGrey}
                enabled={!loading.regions}
              >
                <Picker.Item label="Select region" value={null} />
                {regions.map((r) => (
                  <Picker.Item key={r.id} label={r.name} value={r.id} />
                ))}
              </Picker>
            </View>
            {loading.regions && (
              <ActivityIndicator color={COLORS.primary} style={spacing.mt2} />
            )}
          </View>
        </View>

        <View style={spacing.mb3}>
          <View style={globalStyles.card}>
            <Text style={globalStyles.label}>Country</Text>
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
                selectedValue={selectedCountry}
                onValueChange={(v) => setSelectedCountry(v)}
                style={[
                  globalStyles.pickerText,
                  globalStyles.pickerWeb,
                  { flex: 1 },
                ]}
                dropdownIconColor={COLORS.textGrey}
                enabled={!!selectedRegion && !loading.countries}
              >
                <Picker.Item
                  label={
                    selectedRegion ? "Select country" : "Select region first"
                  }
                  value={null}
                />
                {countries.map((c) => (
                  <Picker.Item key={c.id} label={c.name} value={c.id} />
                ))}
              </Picker>
            </View>
            {loading.countries && (
              <ActivityIndicator color={COLORS.primary} style={spacing.mt2} />
            )}
          </View>
        </View>

        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Full Name"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.name}
          onChangeText={(t) => handleInputChange("name", t)}
        />

        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Email"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="email-address"
          autoCapitalize="none"
          value={formData.email}
          onChangeText={(t) => handleInputChange("email", t)}
        />

        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="City"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.city}
          onChangeText={(t) => handleInputChange("city", t)}
        />

        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Address"
          placeholderTextColor={COLORS.placeholderGrey}
          value={formData.address}
          onChangeText={(t) => handleInputChange("address", t)}
        />

        <TextInput
          style={[globalStyles.input, spacing.mb3]}
          placeholder="Postal Code"
          placeholderTextColor={COLORS.placeholderGrey}
          keyboardType="number-pad"
          value={formData.postalCode}
          onChangeText={(t) => handleInputChange("postalCode", t)}
        />

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
            onChangeText={(t) => handleInputChange("password", t)}
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
            onChangeText={(t) => handleInputChange("confirmPassword", t)}
          />
        </View>

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

      <View style={[globalStyles.row, globalStyles.center, spacing.mt5]}>
        <Text style={[globalStyles.text, spacing.mr1]}>Want to go back?</Text>
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={globalStyles.link}>Go back</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default RegisterScreen;
