import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import { decodeToken } from "../utils/decodeToken";
import { ROUTES } from "../routes";

export const employeeAuthGuard = async () => {
  try {
    const employeeToken = await AsyncStorage.getItem("employeeToken");

    if (!employeeToken) {
      router.replace(ROUTES.LOGIN_EMPLOYEE);
      return false;
    }

    const decoded = decodeToken(employeeToken);

    if (!decoded) {
      await AsyncStorage.removeItem("employeeToken");
      router.replace(ROUTES.LOGIN_EMPLOYEE);
      return false;
    }

    const now = Date.now() / 1000;
    if (decoded.exp && decoded.exp < now) {
      console.log("⚠️ Employee token expired");
      await AsyncStorage.removeItem("employeeToken");
      router.replace(ROUTES.LOGIN_EMPLOYEE);
      return false;
    }

    return true;
  } catch (error) {
    console.error("EmployeeAuthGuard error:", error);
    await AsyncStorage.removeItem("employeeToken");
    router.replace(ROUTES.LOGIN_EMPLOYEE);
    return false;
  }
};
