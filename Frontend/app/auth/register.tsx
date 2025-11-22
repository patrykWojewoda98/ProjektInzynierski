import React, { useEffect, useState } from "react";
import { ActivityIndicator, Image, ScrollView, Text, View } from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles } from "../../assets/styles/styles";
import ApiService from "../../services/api";

const RegisterScreen = () => {
  const [regions, setRegions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchRegions = async () => {
      try {
        const data = await ApiService.getRegions();
        setRegions(data);
      } catch (err) {
        setError(err);
      } finally {
        setLoading(false);
      }
    };

    fetchRegions();
  }, []);

  return (
    <ScrollView
      contentContainerStyle={[
        globalStyles.centerContainer,
        { paddingVertical: 30 },
      ]}
    >
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[
          globalStyles.logo,
          { width: 200, height: 200, marginBottom: 20 },
        ]}
      />

      <Text style={[globalStyles.header, { marginBottom: 30 }]}>
        Zarejestruj się
      </Text>

      {loading ? (
        <ActivityIndicator size="large" color={COLORS.primary} />
      ) : error ? (
        <Text style={{ color: COLORS.error }}>
          Wystąpił błąd podczas ładowania regionów.
        </Text>
      ) : (
        <View style={{ width: "80%" }}>
          <Text
            style={[
              globalStyles.text,
              { marginBottom: 20, textAlign: "center" },
            ]}
          >
            Lista regionów:
          </Text>
          {regions.map((region) => (
            <View
              key={region.id}
              style={{
                backgroundColor: COLORS.darkGrey,
                padding: 15,
                borderRadius: 8,
                marginBottom: 10,
              }}
            >
              <Text style={globalStyles.text}>{region.name}</Text>
            </View>
          ))}
        </View>
      )}
    </ScrollView>
  );
};

export default RegisterScreen;
