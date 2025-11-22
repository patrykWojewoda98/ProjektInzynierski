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
    flexGrow: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: COLORS.background,
    paddingVertical: 20,
    width: "100%",
  },
  formContainer: {
    width: "90%",
    maxWidth: 400,
  },

  // Text styles
  header: {
    color: COLORS.whiteHeader,
    fontSize: 28,
    fontWeight: "bold",
    marginBottom: 10,
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
  label: {
    color: COLORS.textGrey,
    fontSize: 14,
    marginBottom: 8,
    fontWeight: "500",
  },
  errorText: {
    color: COLORS.error,
    fontSize: 14,
    marginTop: 4,
    marginBottom: 8,
  },
  errorContainer: {
    backgroundColor: "rgba(244, 67, 54, 0.1)",
    padding: 12,
    borderRadius: 8,
    marginBottom: 16,
    width: "100%",
  },

  // Form elements
  input: {
    backgroundColor: COLORS.darkGrey,
    borderRadius: 8,
    padding: 14,
    color: COLORS.textGrey,
    fontSize: 16,
    marginBottom: 16,
    borderWidth: 1,
    borderColor: "transparent",
  },
  inputError: {
    borderColor: COLORS.error,
  },
  inputFocused: {
    borderColor: COLORS.primary,
  },
  pickerContainer: {
    backgroundColor: COLORS.darkGrey,
    borderRadius: 8,
    marginBottom: 16,
    borderWidth: 1,
    borderColor: "transparent",
  },
  picker: {
    color: COLORS.textGrey,
    width: "100%",
  },
  pickerItem: {
    color: COLORS.textGrey,
  },
  passwordContainer: {
    position: "relative",
    width: "100%",
  },
  eyeIcon: {
    position: "absolute",
    right: 15,
    top: 15,
    zIndex: 10,
  },

  // Button styles
  button: {
    backgroundColor: COLORS.primary,
    borderRadius: 8,
    paddingVertical: 16,
    alignItems: "center",
    justifyContent: "center",
    marginTop: 8,
    marginBottom: 20,
    width: "100%",
  },
  buttonText: {
    color: COLORS.whiteHeader,
    fontSize: 16,
    fontWeight: "600",
  },
  buttonDisabled: {
    backgroundColor: COLORS.mediumGrey,
  },

  // Image styles
  logo: {
    width: 180,
    height: 180,
    marginBottom: 20,
    resizeMode: "contain",
  },

  // App header
  headerContainer: {
    width: "100%",
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    paddingVertical: 12,
    paddingHorizontal: 16,
    backgroundColor: COLORS.mediumGrey,
  },
  headerLeft: {
    flexDirection: "row",
    alignItems: "center",
  },
  headerLogo: {
    width: 40,
    height: 40,
    resizeMode: "contain",
  },
  headerWelcome: {
    color: COLORS.whiteHeader,
    fontSize: 16,
    marginLeft: 8,
  },
  headerLogoutButton: {
    paddingVertical: 8,
    paddingHorizontal: 12,
    borderRadius: 6,
    backgroundColor: COLORS.primary,
  },
  headerLogoutText: {
    color: COLORS.whiteHeader,
    fontSize: 14,
    fontWeight: "600",
  },

  // Loading indicator
  loadingIndicator: {
    marginVertical: 10,
  },

  // Spacing
  section: {
    marginBottom: 8,
    width: "100%",
  },
  smallSpacing: {
    marginVertical: 8,
  },
  mediumSpacing: {
    marginVertical: 16,
  },
  largeSpacing: {
    marginVertical: 24,
  },
  // Root container for layout
  rootContainer: {
    flex: 1,
    width: "100%",
    backgroundColor: COLORS.background,
  },

  // Main content wrapper (below header)
  contentWrapper: {
    flex: 1,
    width: "100%",
    backgroundColor: COLORS.background,
    padding: 20,
  },
  // Main Menu specific styles
  mainMenuScroll: {
    flexGrow: 1,
    backgroundColor: COLORS.background,
    paddingHorizontal: 20,
    paddingTop: 24,
    paddingBottom: 40,
  },
  mainMenuScrollStyle: {
    backgroundColor: COLORS.background,
  },
  mainMenuTop: {
    alignItems: "center",
    marginBottom: 24,
  },
  mainMenuLogoSmall: {
    width: 80,
    height: 80,
    resizeMode: "contain",
    marginBottom: 12,
  },
  mainMenuTitle: {
    color: COLORS.whiteHeader,
    fontSize: 22,
    fontWeight: "700",
    textAlign: "center",
    marginBottom: 8,
  },
  gridContainer: {
    flexDirection: "column",
    alignItems: "center",
  },
  gridTile: {
    width: "100%",
    maxWidth: 760,
    backgroundColor: COLORS.lightGrey,
    borderRadius: 12,
    paddingVertical: 18,
    paddingHorizontal: 12,
    marginBottom: 16,
    alignItems: "center",
    justifyContent: "center",
    alignSelf: "center",
    // Shadow
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.18,
    shadowRadius: 4,
    elevation: 3,
  },
  gridTileIcon: {
    width: 56,
    height: 56,
    resizeMode: "contain",
    marginBottom: 12,
  },
  gridTileLabel: {
    color: COLORS.whiteHeader,
    fontSize: 14,
    textAlign: "center",
  },
});
