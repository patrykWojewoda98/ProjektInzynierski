import { StyleSheet } from "react-native";
import { COLORS } from "../Constants/colors";

export const globalStyles = StyleSheet.create({
  // Container styles
  container: {
    flex: 1,
    backgroundColor: COLORS.background,
    padding: 20,
  },
  centerContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: COLORS.background,
  },

  // Text styles
  header: {
    color: COLORS.whiteHeader,
    fontSize: 32,
    fontWeight: "bold",
    marginBottom: 20,
    textAlign: "center",
  },
  text: {
    color: COLORS.textGrey,
    fontSize: 16,
    lineHeight: 24,
  },
  smallText: {
    color: COLORS.textGrey,
    fontSize: 14,
    lineHeight: 20,
  },
  placeholder: {
    color: COLORS.placeholderGrey,
    fontSize: 16,
  },

  // Button styles
  button: {
    backgroundColor: COLORS.primary,
    borderRadius: 8,
    paddingVertical: 12,
    paddingHorizontal: 24,
    minWidth: 200,
    alignItems: "center",
    justifyContent: "center",
    marginVertical: 8,
  },
  buttonText: {
    color: COLORS.whiteHeader,
    fontSize: 16,
    fontWeight: "600",
  },

  // Image styles
  logo: {
    width: 150,
    height: 150,
    marginBottom: 40,
    resizeMode: "contain",
  },

  // Spacing
  section: {
    marginVertical: 16,
  },
  smallSpacing: {
    marginVertical: 8,
  },
  mediumSpacing: {
    marginVertical: 16,
  },
  largeSpacing: {
    marginVertical: 32,
  },
});
