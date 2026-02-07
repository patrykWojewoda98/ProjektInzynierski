import { decodeToken } from "@/utils/decodeToken";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useLocalSearchParams, useRouter } from "expo-router";
import React, { useEffect, useState } from "react";
import {
  ActivityIndicator,
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

const FinancialMetric = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();

  const [metric, setMetric] = useState<any>(null);
  const [instrument, setInstrument] = useState<any>(null);
  const [clientId, setClientId] = useState<number | null>(null);

  const [loading, setLoading] = useState(true);
  const [aiLoading, setAiLoading] = useState(false);
  const [comments, setComments] = useState<any[]>([]);
  const [newComment, setNewComment] = useState("");
  const [commentLoading, setCommentLoading] = useState(false);

  useEffect(() => {
    const loadClientId = async () => {
      const token = await AsyncStorage.getItem("userToken");
      const decoded = decodeToken(token);
      setClientId(decoded?.id ? Number(decoded.id) : null);
    };

    loadClientId();
  }, []);

  useEffect(() => {
    if (!id) return;

    const loadData = async () => {
      try {
        const instrumentData = await ApiService.getInvestInstrumentById(
          Number(id),
        );
        setInstrument(instrumentData);

        if (instrumentData?.financialMetricId) {
          const metricData = await ApiService.getFinancialMetricById(
            instrumentData.financialMetricId,
          );
          setMetric(metricData);
        }
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [id]);

  useEffect(() => {
    if (!id) return;

    const loadComments = async () => {
      try {
        const data = await ApiService.getCommentsByInvestInstrumentId(
          Number(id),
        );

        // sort: newest -> oldest
        const sorted = data.sort(
          (a: any, b: any) =>
            new Date(b.dateTime).getTime() - new Date(a.dateTime).getTime(),
        );

        setComments(sorted);
      } catch (e) {
        console.error("Failed to load comments", e);
      }
    };

    loadComments();
  }, [id]);

  const handleAddComment = async () => {
    if (!newComment.trim()) return;

    try {
      setCommentLoading(true);

      await ApiService.createComment({
        content: newComment,
        clientId: clientId,
        investInstrumentId: Number(id),
      });

      setNewComment("");

      // reload comments
      const data = await ApiService.getCommentsByInvestInstrumentId(Number(id));
      const sorted = data.sort(
        (a: any, b: any) =>
          new Date(b.dateTime).getTime() - new Date(a.dateTime).getTime(),
      );
      setComments(sorted);
    } catch (e) {
      console.error("Add comment failed", e);
      alert("Failed to add comment.");
    } finally {
      setCommentLoading(false);
    }
  };

  const handleAIAnalysis = async () => {
    if (!clientId) {
      alert("Client not authenticated.");
      return;
    }

    try {
      setAiLoading(true);

      const request = await ApiService.createAnalysisRequest(
        Number(id),
        Number(clientId),
      );

      if (request.aiAnalysisResultId) {
        router.push({
          pathname: ROUTES.AI_ANALYSIS_RESULT,
          params: { id: request.aiAnalysisResultId },
        });
      } else {
        alert("AI analysis is being processed. Please check later.");
      }
    } catch (e) {
      console.error("AI request failed:", e);
      alert("Failed to generate AI analysis.");
    } finally {
      setAiLoading(false);
    }
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Metrics</Text>

      {/* INSTRUMENT */}
      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <Text style={[globalStyles.cardTitle, spacing.mb2]}>Instrument</Text>

        <Text style={globalStyles.text}>Name: {instrument?.name}</Text>
        <Text style={globalStyles.text}>Ticker: {instrument?.ticker}</Text>
        <Text style={globalStyles.text}>
          Description: {instrument?.description}
        </Text>
        <Text style={globalStyles.text}>
          Market Date: {instrument?.marketDataDate}
        </Text>
      </View>

      {/* METRICS */}
      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        {metric ? (
          <>
            <Text style={[globalStyles.cardTitle, spacing.mb2]}>
              Financial Ratios
            </Text>

            <Text style={globalStyles.text}>P/E: {metric.pe}</Text>
            <Text style={globalStyles.text}>P/B: {metric.pb}</Text>
            <Text style={globalStyles.text}>ROE: {metric.roe}%</Text>
            <Text style={globalStyles.text}>
              Debt to Equity: {metric.debtToEquity}
            </Text>
            <Text style={globalStyles.text}>
              Dividend Yield: {metric.dividendYield}%
            </Text>
          </>
        ) : (
          <Text style={[globalStyles.cardTitle, spacing.mb2]}>
            Financial Ratios – no data available
          </Text>
        )}
      </View>

      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <Text style={[globalStyles.cardTitle, spacing.mb2]}>Comments</Text>

        {comments.length === 0 ? (
          <Text style={globalStyles.textSmall}>No comments yet.</Text>
        ) : (
          comments.map((c) => {
            const isMine = c.clientId === clientId;

            return (
              <View
                key={c.id}
                style={[
                  spacing.mb2,
                  isMine
                    ? globalStyles.commentBubbleMine
                    : globalStyles.commentBubbleOther,
                ]}
              >
                <View style={[globalStyles.row, globalStyles.spaceBetween]}>
                  <Text style={globalStyles.textSmall}>{c.clientName}</Text>
                  <Text style={globalStyles.textSmall}>
                    {new Date(c.dateTime).toLocaleString()}
                  </Text>
                </View>

                <Text style={[globalStyles.text, spacing.mt1]}>
                  {c.content}
                </Text>
              </View>
            );
          })
        )}

        {/* ADD COMMENT */}
        <View style={[globalStyles.row, spacing.mt2]}>
          <TextInput
            style={[
              globalStyles.input,
              globalStyles.flex1,
              spacing.mr1,
              globalStyles.commentInputWrapper,
            ]}
            placeholder="Write a comment..."
            placeholderTextColor={COLORS.textGrey}
            value={newComment}
            onChangeText={setNewComment}
          />

          <TouchableOpacity
            style={[globalStyles.button, globalStyles.buttonSmall]}
            onPress={handleAddComment}
            disabled={commentLoading}
          >
            {commentLoading ? (
              <ActivityIndicator color={COLORS.whiteHeader} />
            ) : (
              <Text style={globalStyles.buttonText}>Add</Text>
            )}
          </TouchableOpacity>
        </View>
      </View>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.py2,
          aiLoading && globalStyles.buttonDisabled,
        ]}
        disabled={aiLoading}
        onPress={handleAIAnalysis}
      >
        {aiLoading ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>AI Investment Insight</Text>
        )}
      </TouchableOpacity>

      <TouchableOpacity
        style={[
          globalStyles.button,
          globalStyles.fullWidth,
          spacing.py2,
          spacing.mt1,
        ]}
        onPress={() =>
          router.push({
            pathname: ROUTES.FINANCIAL_REPORT_BY_INSTRUMENT,
            params: { id: id },
          })
        }
      >
        {aiLoading ? (
          <ActivityIndicator color="#fff" />
        ) : (
          <Text style={globalStyles.buttonText}>
            See Financial Reports Data
          </Text>
        )}
      </TouchableOpacity>

      <TouchableOpacity
        style={[spacing.mt6]}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default FinancialMetric;
