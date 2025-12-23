import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import { decodeToken } from "../utils/decodeToken";
import { ROUTES } from "../routes";

export const authGuard = async () => {
  try {
    const token = await AsyncStorage.getItem("userToken");

    //Brak tokenu → przekierowanie
    if (!token) {
      router.replace(ROUTES.LOGIN);
      return false;
    }

    //Próba dekodowania
    const decoded = decodeToken(token);
    if (!decoded) {
      await AsyncStorage.removeItem("userToken");
      router.replace(ROUTES.LOGIN);
      return false;
    }

    //Sprawdzenie wygaśnięcia
    const now = Date.now() / 1000;
    if (decoded.exp && decoded.exp < now) {
      console.log("⚠️ Token expired");
      await AsyncStorage.removeItem("userToken");
      router.replace(ROUTES.LOGIN);
      return false;
    }

    //Token ważny
    return true;
  } catch (error) {
    console.error("AuthGuard error:", error);
    router.replace(ROUTES.LOGIN);
    return false;
  }
};
