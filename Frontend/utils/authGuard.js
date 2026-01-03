import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import { decodeToken } from "../utils/decodeToken";
import { ROUTES } from "../routes";

export const authGuard = async () => {
  try {
    const userToken = await AsyncStorage.getItem("userToken");
    const employeeToken = await AsyncStorage.getItem("employeeToken");

    if (!userToken && !employeeToken) {
      router.replace("/");
      return false;
    }

    const token = employeeToken ?? userToken;
    const decoded = decodeToken(token);

    if (!decoded) {
      await AsyncStorage.multiRemove(["userToken", "employeeToken"]);
      router.replace(ROUTES.LOGIN);
      return false;
    }

    const now = Date.now() / 1000;

    if (decoded.exp && decoded.exp < now) {
      console.log("⚠️ Token expired");
      await AsyncStorage.multiRemove(["userToken", "employeeToken"]);
      router.replace("/");
      return false;
    }

    return true;
  } catch (error) {
    console.error("AuthGuard error:", error);
    router.replace(ROUTES.LOGIN);
    return false;
  }
};
