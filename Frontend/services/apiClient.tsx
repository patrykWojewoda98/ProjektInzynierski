import axios from "axios";
import Constants from "expo-constants";
import { Alert, Platform } from "react-native";
import { COLORS } from "../assets/Constants/colors";

const getApiBaseUrl = () => {
  if (Platform.OS === "web") {
    return "http://localhost:5036/api";
  }

  if (Platform.OS === "android") {
    return "http://10.0.2.2:5036/api";
  }

  const host = Constants.expoConfig?.hostUri?.split(":")[0];
  return host ? `http://${host}:5036/api` : "http://localhost:5036/api";
};

const api = axios.create({
  baseURL: getApiBaseUrl(),
});

function showWebErrorModal(title, errors) {
  const existing = document.getElementById("global-error-modal");
  if (existing) existing.remove();

  const overlay = document.createElement("div");
  overlay.id = "global-error-modal";
  overlay.style.position = "fixed";
  overlay.style.top = "0";
  overlay.style.left = "0";
  overlay.style.width = "100%";
  overlay.style.height = "100%";
  overlay.style.backgroundColor = "rgba(0,0,0,0.7)";
  overlay.style.display = "flex";
  overlay.style.justifyContent = "center";
  overlay.style.alignItems = "center";
  overlay.style.zIndex = "9999";

  const modal = document.createElement("div");
  modal.style.background = COLORS.darkGrey;
  modal.style.padding = "32px";
  modal.style.borderRadius = "12px";
  modal.style.width = "90%";
  modal.style.maxWidth = "500px";
  modal.style.boxShadow = "0 20px 40px rgba(0,0,0,0.5)";
  modal.style.color = COLORS.textGrey;
  modal.style.fontFamily = "sans-serif";

  const header = document.createElement("h2");
  header.innerText = title;
  header.style.color = COLORS.whiteHeader;
  header.style.marginBottom = "20px";
  header.style.fontSize = "22px";

  const list = document.createElement("ul");
  list.style.paddingLeft = "20px";
  list.style.marginBottom = "24px";

  errors.forEach((e) => {
    const li = document.createElement("li");
    li.innerText = e;
    li.style.marginBottom = "6px";
    li.style.fontSize = "15px";
    list.appendChild(li);
  });

  const button = document.createElement("button");
  button.innerText = "OK";
  button.style.backgroundColor = COLORS.primary;
  button.style.color = COLORS.whiteHeader;
  button.style.border = "none";
  button.style.padding = "10px 24px";
  button.style.borderRadius = "8px";
  button.style.cursor = "pointer";
  button.style.fontWeight = "600";
  button.style.fontSize = "14px";

  button.onmouseenter = () => {
    button.style.opacity = "0.85";
  };

  button.onmouseleave = () => {
    button.style.opacity = "1";
  };

  button.onclick = () => overlay.remove();

  modal.appendChild(header);
  modal.appendChild(list);
  modal.appendChild(button);
  overlay.appendChild(modal);
  document.body.appendChild(overlay);
}

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const errorData = error.response?.data;

    let title = "Error";
    let errors = ["Something went wrong"];

    if (errorData?.errors) {
      title = errorData.message || "Validation failed";
      errors = Object.values(errorData.errors).flat();
    } else if (errorData?.message) {
      title = errorData.message;
      errors = [errorData.message];
    }

    if (Platform.OS === "web") {
      showWebErrorModal(title, errors);
    } else {
      Alert.alert(title, errors.join("\n"));
    }

    return Promise.reject(error);
  },
);

export default api;
