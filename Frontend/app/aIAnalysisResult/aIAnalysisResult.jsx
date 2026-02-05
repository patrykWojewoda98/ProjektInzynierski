import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const AIAnalysisResult = () => {
  const router = useRouter();
  const { id } = useLocalSearchParams();
  const { itemWidth } = useResponsiveColumns();

  const analysisId = Array.isArray(id) ? id[0] : id;

  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!analysisId) return;

    const loadResult = async () => {
      try {
        const data = await ApiService.getAIAnalysisResultById(
          Number(analysisId),
        );
        setResult(data);
      } catch (error) {
        console.error("Error loading AI analysis result:", error);
      } finally {
        setLoading(false);
      }
    };

    loadResult();
  }, [analysisId]);

  const handleDownloadPdf = async () => {
    try {
      const pdfBlob = await ApiService.generateInvestmentRecommendationPdf(
        Number(analysisId),
      );

      await savePdfWithPicker(Number(analysisId));
    } catch (error) {
      console.error("Error downloading PDF:", error);
    }
  };
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

  const renderCard = (title, content) => (
    <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
      <Text style={[globalStyles.cardTitle, spacing.mb2]}>{title}</Text>
      <Text style={globalStyles.text}>{content}</Text>
    </View>
  );

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        AI Investment Analysis
      </Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        {renderCard("Summary", result.summary)}
        {renderCard("Recommendation", result.recommendation)}
        {renderCard("Key Insights", result.keyInsights)}
        {renderCard("Confidence Score", `${result.confidenceScore}%`)}
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
