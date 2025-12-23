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

const InvestInstrument = () => {
  const router = useRouter();
  const [regions, setRegions] = useState([]);
  const [countries, setCountries] = useState([]);
  const [selectedSector, setSelectedSector] = useState<number | null>(null);
  const [selectedRegion, setSelectedRegion] = useState<number | null>(null);
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

  //temp data
  const sectors = [
    { id: 1, name: "Technology" },
    { id: 2, name: "Healthcare" },
    { id: 3, name: "Finance" },
  ];

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

  // Fetch Sectors
  useEffect(() => {
    // Impement fetching sectors
  }, [selectedSector]);

  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors.length > 0) setErrors([]);
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

      <Text style={[globalStyles.header, spacing.mb4]}>Invest Instruments</Text>

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

        {/* Sector Picker */}
        <Text style={globalStyles.label}>Sector</Text>

        {/* Type Picker */}
        <Text style={globalStyles.label}>Type</Text>

        {/* Main Menu Link */}
        <View style={[globalStyles.row, globalStyles.center, spacing.mt4]}>
          <Text style={[globalStyles.text, spacing.mr1]}>
            Go back to main menu
          </Text>
          <TouchableOpacity
            onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
          >
            <Text style={globalStyles.link}>Main Menu</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

export default InvestInstrument;
