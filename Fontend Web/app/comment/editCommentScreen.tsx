import { Ionicons } from "@expo/vector-icons";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
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
import { confirmAction } from "../../utils/confirmAction";
import { useResponsiveColumns } from "../../utils/useResponsiveColumns";

const EditCommentScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const { itemWidth } = useResponsiveColumns();

  const [comment, setComment] = useState<any>(null);
  const [clientName, setClientName] = useState("");
  const [content, setContent] = useState("");
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    const load = async () => {
      try {
        const data = await ApiService.getCommentById(Number(id));
        setComment(data);
        setContent(data.content);

        const client = await ApiService.getClientById(data.clientId);
        setClientName(client.name);
      } catch {
        Alert.alert("Error", "Failed to load comment.");
        router.back();
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [id]);

  const handleSave = async () => {
    if (!content.trim()) {
      Alert.alert("Validation error", "Comment content cannot be empty.");
      return;
    }

    setSaving(true);
    try {
      await ApiService.updateComment(comment.id, { content });
      Alert.alert("Success", "Comment updated successfully.");
      router.back();
    } catch {
      Alert.alert("Error", "Failed to update comment.");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = () => {
    confirmAction({
      title: "Confirm delete",
      message: "Delete this comment?",
      onConfirm: async () => {
        try {
          await ApiService.deleteComment(comment.id);
          router.replace(ROUTES.COMMENT_MODERATION);
        } catch {
          Alert.alert("Error", "Failed to delete comment.");
        }
      },
    });
  };

  if (loading) {
    return <ActivityIndicator color={COLORS.primary} />;
  }

  if (!comment) {
    return (
      <View style={globalStyles.centerContainer}>
        <Text style={globalStyles.text}>Comment not found.</Text>
      </View>
    );
  }

  return (
    <ScrollView contentContainerStyle={globalStyles.scrollContainer}>
      <Text style={[globalStyles.header, spacing.mb4]}>Edit Comment</Text>

      <View
        style={[
          globalStyles.row,
          { flexWrap: "wrap", justifyContent: "center", width: "100%" },
        ]}
      >
        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.textSmall}>Client</Text>
          <Text style={globalStyles.text}>{clientName}</Text>
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.textSmall}>Instrument ID</Text>
          <Text style={globalStyles.text}>{comment.investInstrumentID}</Text>
        </View>

        <View style={[globalStyles.card, spacing.m2, { width: itemWidth }]}>
          <Text style={globalStyles.textSmall}>Date</Text>
          <Text style={globalStyles.text}>
            {new Date(comment.dateTime).toLocaleString()}
          </Text>
        </View>
      </View>

      <View style={[globalStyles.card, globalStyles.fullWidth, spacing.mb4]}>
        <Text style={globalStyles.label}>Comment content</Text>
        <TextInput
          style={[
            globalStyles.input,
            { minHeight: 120, textAlignVertical: "top" },
          ]}
          multiline
          value={content}
          onChangeText={setContent}
          placeholder="Edit comment..."
          placeholderTextColor={COLORS.textGrey}
        />
      </View>

      {/* ACTIONS */}
      <View style={[globalStyles.card, globalStyles.fullWidth]}>
        <TouchableOpacity
          style={[
            globalStyles.button,
            spacing.mb2,
            saving && globalStyles.buttonDisabled,
          ]}
          disabled={saving}
          onPress={handleSave}
        >
          {saving ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={globalStyles.buttonText}>Save changes</Text>
          )}
        </TouchableOpacity>

        <TouchableOpacity style={globalStyles.button} onPress={handleDelete}>
          <View style={[globalStyles.row, globalStyles.center]}>
            <Ionicons
              name="trash"
              size={20}
              color={COLORS.whiteHeader}
              style={spacing.mr2}
            />
            <Text style={globalStyles.buttonText}>Delete comment</Text>
          </View>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default EditCommentScreen;
