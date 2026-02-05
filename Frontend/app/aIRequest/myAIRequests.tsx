import { confirmAction } from "@/utils/confirmAction";
import { Ionicons } from "@expo/vector-icons";
import * as FileSystem from "expo-file-system/legacy";
import { useRouter } from "expo-router";
import * as Sharing from "expo-sharing";
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
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";
import { AuthContext } from "../_layout";

const MyAIRequests = () => {
  const router = useRouter();
  const { user } = useContext(AuthContext);
  const { itemWidth } = useResponsiveColumns();

  const [requests, setRequests] = useState<any[]>([]);
  const [instrumentNames, setInstrumentNames] = useState<
    Record<number, string>
  >({});
  const [loading, setLoading] = useState(true);
  const [downloadingId, setDownloadingId] = useState<number | null>(null);

  useEffect(() => {
    const loadData = async () => {
      if (!user?.id) return;

      try {
        const aiRequests = await ApiService.getAnalysisRequestsByClientId(
          user.id,
        );
        setRequests(aiRequests);

        const uniqueInstrumentIds = [
          ...new Set(aiRequests.map((r: any) => r.investInstrumentId)),
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
        Alert.alert("Error", "Nie udało się załadować danych");
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [user]);

  const handleDeleteRequest = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this AI analysis request?",
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

  const handleDownloadPdf = async (analysisRequestId: number) => {
    try {
      setDownloadingId(analysisRequestId);

      const pdfUri =
        await ApiService.generateInvestmentRecommendationPdf(analysisRequestId);

      await Sharing.shareAsync(pdfUri);
    } catch (error) {
      console.error("Error downloading PDF:", error);
      Alert.alert("Error", "Failed to download PDF.");
    } finally {
      setDownloadingId(null);
    }
  };
  const handleSavePdfWithPicker = async (id: number) => {
    try {
      // 1️⃣ pobierz PDF do sandboxu
      const tempUri =
        await ApiService.downloadInvestmentRecommendationPdfToTemp(id);

      // 2️⃣ otwórz systemowy picker folderu (TU JEST RÓŻNICA)
      const permissions =
        await FileSystem.StorageAccessFramework.requestDirectoryPermissionsAsync();

      if (!permissions.granted) {
        return;
      }

      // 3️⃣ utwórz nowy plik (tu pojawia się "Zapisz")
      const fileUri = await FileSystem.StorageAccessFramework.createFileAsync(
        permissions.directoryUri,
        `Investment_Recommendation_${id}.pdf`,
        "application/pdf",
      );

      // 4️⃣ skopiuj zawartość
      const pdfBase64 = await FileSystem.readAsStringAsync(tempUri, {
        encoding: FileSystem.EncodingType.Base64,
      });

      await FileSystem.writeAsStringAsync(fileUri, pdfBase64, {
        encoding: FileSystem.EncodingType.Base64,
      });

      Alert.alert("Saved", "PDF saved successfully");
    } catch (error) {
      console.error("Save as failed:", error);
      Alert.alert("Error", "Failed to save PDF");
    }
  };

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
        <View
          style={[
            globalStyles.row,
            { flexWrap: "wrap", justifyContent: "center", width: "100%" },
          ]}
        >
          {requests.map((req) => (
            <View
              key={req.id}
              style={[globalStyles.card, spacing.m2, { width: itemWidth }]}
            >
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

              <Text style={globalStyles.textSmall}>
                Created at: {new Date(req.createdAt).toLocaleDateString()}
              </Text>

              <Text style={globalStyles.textSmall}>
                Status: {req.aiAnalysisResultId ? "Completed" : "Pending"}
              </Text>

              {req.aiAnalysisResultId && (
                <View style={spacing.mt4}>
                  {/* SHARE */}
                  <TouchableOpacity
                    style={[globalStyles.button, spacing.mb2]}
                    onPress={() => handleDownloadPdf(req.id)}
                    disabled={downloadingId === req.id}
                  >
                    {downloadingId === req.id ? (
                      <ActivityIndicator color="#fff" />
                    ) : (
                      <Text style={globalStyles.primaryButtonText}>
                        Share Investment Report (PDF)
                      </Text>
                    )}
                  </TouchableOpacity>

                  <TouchableOpacity
                    style={globalStyles.secondaryButton}
                    onPress={() => handleSavePdfWithPicker(req.id)}
                  >
                    <Text style={globalStyles.secondaryButtonText}>
                      Save as…
                    </Text>
                  </TouchableOpacity>
                </View>
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
