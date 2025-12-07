import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import { decodeToken } from "../utils/decodeToken"; // ← używamy nowego helpera

export const authGuard = async () => {
  try {
    const token = await AsyncStorage.getItem("userToken");

    // Brak tokenu → login
    if (!token) {
      router.replace("/auth/login");
      return false;
    }

    // Dekodowanie przez helper
    const decoded = decodeToken(token);

    // Jeśli nie udało się zdekodować → login
    if (!decoded) {
      await AsyncStorage.removeItem("userToken");
      router.replace("/auth/login");
      return false;
    }

    // Sprawdzenie wygaśnięcia tokenu
    const now = Date.now() / 1000;

    if (decoded.exp && decoded.exp < now) {
      await AsyncStorage.removeItem("userToken");
      router.replace("/auth/login");
      return false;
    }

    // Token poprawny
    return true;
  } catch (error) {
    console.error("AuthGuard error:", error);
    router.replace("/auth/login");
    return false;
  }
};
