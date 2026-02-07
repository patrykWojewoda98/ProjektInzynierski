import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import { decodeToken } from "../utils/decodeToken";
import { ROUTES } from "../routes";

export const authGuard = async () => {
  try {
    const userToken = await AsyncStorage.getItem("userToken");

    if (!userToken) {
      router.replace(ROUTES.LOGIN);
      return false;
    }

    const decoded = decodeToken(userToken);

    if (!decoded) {
      await AsyncStorage.removeItem("userToken");
      router.replace(ROUTES.LOGIN);
      return false;
    }

    const now = Date.now() / 1000;
    if (decoded.exp && decoded.exp < now) {
      console.log("⚠️ Client token expired");
      await AsyncStorage.removeItem("userToken");
      router.replace(ROUTES.LOGIN);
      return false;
    }

    return true;
  } catch (error) {
    console.error("ClientAuthGuard error:", error);
    await AsyncStorage.removeItem("userToken");
    router.replace(ROUTES.LOGIN);
    return false;
  }
};
