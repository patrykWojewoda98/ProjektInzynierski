import { Ionicons } from "@expo/vector-icons";
import { Picker } from "@react-native-picker/picker";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
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
import { confirmAction } from "../../utils/confirmAction";

const CommentModerationScreen = () => {
  const router = useRouter();

  const [comments, setComments] = useState<any[]>([]);
  const [clients, setClients] = useState<any[]>([]);
  const [instruments, setInstruments] = useState<any[]>([]);

  const [loading, setLoading] = useState(true);

  const [selectedClientId, setSelectedClientId] = useState<number | "all">(
    "all",
  );
  const [selectedInstrumentId, setSelectedInstrumentId] = useState<
    number | "all"
  >("all");

  // 📥 LOAD CLIENTS + INSTRUMENTS
  useEffect(() => {
    const loadInitialData = async () => {
      const [clientsData, instrumentsData] = await Promise.all([
        ApiService.getAllClients(),
        ApiService.getInvestInstruments(),
      ]);

      setClients(clientsData);
      setInstruments(instrumentsData);
    };

    loadInitialData();
  }, []);

  // 📥 LOAD COMMENTS (FILTERED)
  useEffect(() => {
    loadComments();
  }, [selectedClientId, selectedInstrumentId]);

  const loadComments = async () => {
    setLoading(true);
    try {
      let data: any[] = [];

      if (selectedClientId !== "all" && selectedInstrumentId !== "all") {
        // oba wybrane → filtr front
        const all = await ApiService.getAllComments();
        data = all.filter(
          (c) =>
            c.clientId === selectedClientId &&
            c.investInstrumentID === selectedInstrumentId,
        );
      } else if (selectedClientId !== "all") {
        data = await ApiService.getCommentsByClientId(selectedClientId);
      } else if (selectedInstrumentId !== "all") {
        data =
          await ApiService.getCommentsByInvestInstrumentId(
            selectedInstrumentId,
          );
      } else {
        data = await ApiService.getAllComments();
      }

      setComments(data);
    } finally {
      setLoading(false);
    }
  };

  const getClientName = (id: number) =>
    clients.find((c) => c.id === id)?.name ?? "Unknown client";

  const getInstrumentName = (id: number) =>
    instruments.find((i) => i.id === id)?.name ?? "Unknown instrument";

  const handleDeleteComment = (id: number) => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this comment?",
      onConfirm: async () => {
        try {
          await ApiService.deleteComment(id);
          setComments((prev) => prev.filter((c) => c.id !== id));
        } catch {
          Alert.alert("Error", "Failed to delete comment.");
        }
      },
    });
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Financial Comments</Text>

      {/* 🔽 FILTERS */}
      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <Text style={globalStyles.label}>Investment Instrument</Text>
        <Picker
          selectedValue={selectedInstrumentId}
          onValueChange={(v) => setSelectedInstrumentId(v)}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="Show all" value="all" />
          {instruments.map((i) => (
            <Picker.Item key={i.id} label={i.name} value={i.id} />
          ))}
        </Picker>

        <Text style={[globalStyles.label, spacing.mt3]}>Client</Text>
        <Picker
          selectedValue={selectedClientId}
          onValueChange={(v) => setSelectedClientId(v)}
          style={globalStyles.pickerText}
          dropdownIconColor={COLORS.textGrey}
        >
          <Picker.Item label="Show all" value="all" />
          {clients.map((c) => (
            <Picker.Item key={c.id} label={c.name} value={c.id} />
          ))}
        </Picker>
      </View>

      {/* 🔄 LOADING */}
      {loading && <ActivityIndicator color={COLORS.primary} />}

      {/* ❌ NO COMMENTS */}
      {!loading && comments.length === 0 && (
        <View style={[globalStyles.card, globalStyles.fullWidth]}>
          <Text style={[globalStyles.title, { textAlign: "center" }]}>
            No comments found.
          </Text>
        </View>
      )}

      {/* 💬 COMMENTS LIST */}
      {!loading &&
        comments.map((c) => (
          <View
            key={c.id}
            style={[globalStyles.card, globalStyles.fullWidth, spacing.mb3]}
          >
            <Text style={globalStyles.cardTitle}>
              {getInstrumentName(c.investInstrumentID)}
            </Text>

            <Text style={globalStyles.textSmall}>
              Author: {getClientName(c.clientId)}
            </Text>

            <Text style={[globalStyles.text, spacing.mt2]}>{c.content}</Text>

            <Text style={[globalStyles.textSmall, spacing.mt1]}>
              {new Date(c.dateTime).toLocaleString()}
            </Text>

            <View
              style={[
                globalStyles.row,
                spacing.mt3,
                { justifyContent: "center" },
              ]}
            >
              <TouchableOpacity
                style={spacing.mr4}
                onPress={() =>
                  router.push({
                    pathname: ROUTES.EDIT_COMMENT,
                    params: { id: c.id },
                  })
                }
              >
                <Ionicons name="pencil" size={22} color={COLORS.primary} />
              </TouchableOpacity>

              <TouchableOpacity onPress={() => handleDeleteComment(c.id)}>
                <Ionicons name="trash" size={22} color={COLORS.error} />
              </TouchableOpacity>
            </View>
          </View>
        ))}
    </ScrollView>
  );
};

export default CommentModerationScreen;
