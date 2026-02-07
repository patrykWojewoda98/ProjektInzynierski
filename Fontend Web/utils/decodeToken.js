import { jwtDecode } from "jwt-decode";

export const decodeToken = (token) => {
  try {
    if (!token) {
      console.log("Token is empty!");
      return null;
    }
    const decoded = jwtDecode(token);

    return {
      id: decoded.id || decoded.sub || null,
      name: decoded.name || "",
      exp: decoded.exp,
      isAdmin: decoded.isAdmin || false,
    };
  } catch (error) {
    console.log("Token decode error:", error);
    return null;
  }
};
