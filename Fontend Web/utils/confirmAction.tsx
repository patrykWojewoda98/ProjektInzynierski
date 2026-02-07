import { Alert, Platform } from "react-native";

type ConfirmActionOptions = {
  title?: string;
  message?: string;
  onConfirm: () => void | Promise<void>;
};

export const confirmAction = ({
  title = "Confirm action",
  message = "Are you sure you want to continue?",
  onConfirm,
}: ConfirmActionOptions) => {
  if (Platform.OS === "web") {
    const confirmed = window.confirm(message);
    if (confirmed) {
      onConfirm();
    }
    return;
  }

  Alert.alert(title, message, [
    { text: "Cancel", style: "cancel" },
    { text: "Confirm", style: "destructive", onPress: onConfirm },
  ]);
};
