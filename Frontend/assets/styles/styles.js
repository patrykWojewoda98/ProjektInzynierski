import { StyleSheet } from "react-native";
import { COLORS } from "../Constants/colors";

// Utility function to generate spacing styles
const createSpacing = (base = 4) => {
  const spacing = {};
  for (let i = 0; i <= 10; i++) {
    spacing[`p${i}`] = { padding: base * i };
    spacing[`pt${i}`] = { paddingTop: base * i };
    spacing[`pr${i}`] = { paddingRight: base * i };
    spacing[`pb${i}`] = { paddingBottom: base * i };
    spacing[`pl${i}`] = { paddingLeft: base * i };
    spacing[`px${i}`] = { paddingHorizontal: base * i };
    spacing[`py${i}`] = { paddingVertical: base * i };
    spacing[`m${i}`] = { margin: base * i };
    spacing[`mt${i}`] = { marginTop: base * i };
    spacing[`mr${i}`] = { marginRight: base * i };
    spacing[`mb${i}`] = { marginBottom: base * i };
    spacing[`ml${i}`] = { marginLeft: base * i };
    spacing[`mx${i}`] = { marginHorizontal: base * i };
    spacing[`my${i}`] = { marginVertical: base * i };
  }
  return spacing;
};

export const globalStyles = StyleSheet.create({
  // Layout
  container: {
    flex: 1,
    backgroundColor: COLORS.background,
  },
  centerContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  row: {
    flexDirection: "row",
  },
  col: {
    flexDirection: "column",
  },
  center: {
    justifyContent: "center",
    alignItems: "center",
  },
  spaceBetween: {
    justifyContent: "space-between",
  },
  flex1: {
    flex: 1,
  },
  flexGrow1: {
    flexGrow: 1,
  },
  selfStretch: {
    alignSelf: "stretch",
  },
  fullWidth: {
    width: "100%",
  },

  // Scroll view layout
  scrollContainer: {
    flexGrow: 1,
    backgroundColor: COLORS.background,
    paddingVertical: 40,
    alignItems: "center",
  },

  // Picker wrapper
  pickerWrapper: {
    backgroundColor: COLORS.darkGrey,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: "transparent",
    width: "100%",
  },
  pickerText: {
    color: COLORS.textGrey,
  },

  // Text
  text: {
    color: COLORS.textGrey,
    fontSize: 16,
    lineHeight: 24,
  },
  header: {
    color: COLORS.whiteHeader,
    fontSize: 32,
    fontWeight: "bold",
    textAlign: "center",
  },
  title: {
    fontSize: 24,
    fontWeight: "600",
    color: COLORS.whiteHeader,
    marginBottom: 16,
  },
  subtitle: {
    fontSize: 18,
    color: COLORS.textGrey,
    marginBottom: 12,
  },
  label: {
    color: COLORS.textGrey,
    fontSize: 16,
    marginBottom: 8,
  },
  link: {
    color: COLORS.primary,
    fontWeight: "600",
  },

  // Buttons
  button: {
    backgroundColor: COLORS.primary,
    borderRadius: 8,
    paddingVertical: 12,
    paddingHorizontal: 24,
    alignItems: "center",
    justifyContent: "center",
  },
  buttonText: {
    color: COLORS.whiteHeader,
    fontSize: 16,
    fontWeight: "600",
  },
  buttonDisabled: {
    opacity: 0.7,
  },
  buttonSmall: {
    paddingVertical: 6,
    paddingHorizontal: 12,
    minWidth: 80,
  },
  buttonPressed: {
    opacity: 0.8,
  },
  secondaryButton: {
    backgroundColor: COLORS.secondary,
  },

  // Forms
  input: {
    backgroundColor: COLORS.darkGrey,
    borderRadius: 8,
    padding: 15,
    color: COLORS.whiteHeader,
    fontSize: 16,
  },
  formContainer: {
    width: "90%",
    maxWidth: 400,
  },

  // Header bar
  topHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    backgroundColor: COLORS.primary,
    paddingVertical: 1,
    paddingHorizontal: 16,
  },

  // Icons
  icon: {
    width: 24,
    height: 24,
    resizeMode: "contain",
  },
  menuIcon: {
    width: 50,
    height: 50,
    marginBottom: 10,
    resizeMode: "contain",
  },

  // Logo
  logo: {
    width: 150,
    height: 150,
    resizeMode: "contain",
  },
  logoLarge: {
    width: 250,
    height: 250,
    marginBottom: 40,
  },
  logoSmall: {
    width: 80,
    height: 80,
  },

  // Alignment
  alignCenter: {
    alignItems: "center",
  },

  // Main Menu styles
  menuGrid: {
    flexDirection: "row",
    flexWrap: "wrap",
    justifyContent: "space-around",
    alignItems: "center",
    width: "100%",
    paddingHorizontal: 16,
    paddingBottom: 40,
  },

  menuTile: {
    backgroundColor: COLORS.darkGrey,
    borderRadius: 12,
    width: 150,
    height: 150,
    alignItems: "center",
    justifyContent: "center",
    marginVertical: 8,
    shadowColor: "#000",
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 3,
  },

  menuLabel: {
    color: COLORS.whiteHeader,
    fontSize: 14,
    fontWeight: "600",
    textAlign: "center",
  },

  // Spacing utilities
  ...createSpacing(8), // 8px base spacing

  // Specific spacing utilities
  containerPadding: {
    padding: 20,
  },
  section: {
    marginVertical: 16,
  },
  errorContainer: {
    backgroundColor: COLORS.errorBackground,
    padding: 12,
    borderRadius: 8,
    marginBottom: 16,
  },
  errorText: {
    color: COLORS.error,
    fontSize: 14,
    marginTop: 4,
  },

  card: {
    backgroundColor: COLORS.darkGrey,
    borderRadius: 12,
    padding: 16,
    marginBottom: 12,
    shadowColor: "#000",
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 3,
  },

  cardTitle: {
    color: COLORS.whiteHeader,
    fontSize: 18,
    fontWeight: "600",
    marginBottom: 4,
  },

  textSmall: {
    color: COLORS.textGrey,
    fontSize: 14,
    lineHeight: 20,
  },
});

// Export individual spacing utilities
export const spacing = createSpacing(8);
