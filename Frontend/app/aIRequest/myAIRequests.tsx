import { confirmAction } from "@/utils/confirmAction";
import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { useContext, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles, spacing } from "../../assets/styles/styles";
import { ROUTES } from "../../routes";
import ApiService from "../../services/api";
import { AuthContext } from "../_layout";

const MyAIRequests = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);

  const [requests, setRequests] = useState([]);
  const [instrumentNames, setInstrumentNames] = useState({});

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadData = async () => {
      if (!user?.id) return;

      try {
        const aiRequests = await ApiService.getAnalysisRequestsByClientId(
          user.id,
        );
        setRequests(aiRequests);

        const uniqueInstrumentIds = [
          ...new Set(aiRequests.map((r) => r.investInstrumentId)),
        ];

        const namesMap: Record<number, string> = {};

        await Promise.all(
          uniqueInstrumentIds.map(async (id) => {
            const instrument = await ApiService.getInvestInstrumentById(id);
            namesMap[id] = instrument.name;
          }),
        );

        setInstrumentNames(namesMap);
      } catch (error) {
        console.error("Error loading AI requests:", error);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [user]);

  const handleDeleteRequest = (id) => {
    confirmAction({
      onConfirm: async () => {
        try {
          await ApiService.deleteAIAnalysisRequest(id);
          setRequests((prev) => prev.filter((r) => r.id !== id));
        } catch {
          Alert.alert("Error", "Something went wrong.");
        }
      },
    });
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>
        My AI Analysis Requests
      </Text>

      {requests.length === 0 ? (
        <Text style={globalStyles.text}>
          You don’t have any AI analysis requests yet.
        </Text>
      ) : (
        <View style={globalStyles.fullWidth}>
          {requests.map((req) => (
            <View key={req.id} style={[globalStyles.card, spacing.mb3]}>
              {/* HEADER */}
              <View
                style={[
                  globalStyles.row,
                  globalStyles.spaceBetween,
                  spacing.mb1,
                ]}
              >
                <Text style={globalStyles.cardTitle}>
                  {instrumentNames[req.investInstrumentId] ?? "Loading..."}
                </Text>

                <TouchableOpacity onPress={() => handleDeleteRequest(req.id)}>
                  <Ionicons name="trash" size={20} color={COLORS.error} />
                </TouchableOpacity>
              </View>

              {/* DETAILS */}
              <Text style={globalStyles.textSmall}>
                Created at: {new Date(req.createdAt).toLocaleDateString()}
              </Text>

              <Text style={globalStyles.textSmall}>
                Status: {req.aiAnalysisResultId ? "Completed" : "Pending"}
              </Text>

              {req.aiAnalysisResultId && (
                <TouchableOpacity
                  style={spacing.mt2}
                  onPress={() =>
                    router.push({
                      pathname: ROUTES.AI_ANALYSIS_RESULT,
                      params: { id: req.aiAnalysisResultId },
                    })
                  }
                >
                  <Text style={globalStyles.link}>View Analysis Result</Text>
                </TouchableOpacity>
              )}
            </View>
          ))}
        </View>
      )}

      <TouchableOpacity
        style={spacing.mt6}
        onPress={() => router.push({ pathname: ROUTES.MAIN_MENU })}
      >
        <Text style={globalStyles.link}>Back to Main Menu</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

export default MyAIRequests;
