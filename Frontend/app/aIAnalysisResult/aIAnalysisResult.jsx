import React, { useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";

import ApiService from "../../services/api";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { COLORS } from "../../assets/Constants/colors";
import { ROUTES } from "../../routes";

const AIAnalysisResult = () => {
  const router = useRouter();
  const { id } = useLocalSearchParams();

  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadResult = async () => {
      try {
        const data = await ApiService.getAIAnalysisResultById(id);
        setResult(data);
      } catch (error) {
        console.error("Error loading AI analysis result:", error);
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      loadResult();
    }
  }, [id]);

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  if (!result) {
    return (
      <View style={globalStyles.centerContainer}>
        <Text style={globalStyles.text}>Analysis result not available.</Text>
      </View>
    );
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        AI Investment Analysis
      </Text>

      {/* SUMMARY */}
      <View style={[globalStyles.card, spacing.mb3]}>
        <Text style={globalStyles.cardTitle}>Summary</Text>
        <Text style={globalStyles.text}>{result.summary}</Text>
      </View>

      {/* RECOMMENDATION */}
      <View style={[globalStyles.card, spacing.mb3]}>
        <Text style={globalStyles.cardTitle}>Recommendation</Text>
        <Text style={globalStyles.text}>{result.recommendation}</Text>
      </View>

      {/* KEY INSIGHTS */}
      <View style={[globalStyles.card, spacing.mb3]}>
        <Text style={globalStyles.cardTitle}>Key Insights</Text>
        <Text style={globalStyles.text}>{result.keyInsights}</Text>
      </View>

      {/* CONFIDENCE */}
      <View style={[globalStyles.card, spacing.mb3]}>
        <Text style={globalStyles.cardTitle}>Confidence Score</Text>
        <Text style={globalStyles.text}>{result.confidenceScore}%</Text>
      </View>

      <TouchableOpacity
        style={spacing.mt6}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default AIAnalysisResult;
