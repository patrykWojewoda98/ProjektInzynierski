import React from "react";
import {
  Image,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { COLORS } from "../../assets/Constants/colors";
import { globalStyles } from "../../assets/styles/styles";

const LoginScreen = ({ navigation }) => {
  const [email, setEmail] = React.useState("");
  const [password, setPassword] = React.useState("");

  const handleLogin = () => {
    // Login logic will be implemented later
    console.log("Login attempt with:", { email, password });
  };

  return (
    <ScrollView contentContainerStyle={globalStyles.centerContainer}>
      <Image
        source={require("../../assets/images/Logo.png")}
        style={[globalStyles.logo, { width: 250, height: 250 }]}
      />

      <Text style={[globalStyles.header, { marginBottom: 40 }]}>Login</Text>

      <View style={[globalStyles.section, { width: "80%" }]}>
        <Text style={[globalStyles.text, { marginBottom: 8 }]}>
          Email Adress
        </Text>
        <TextInput
          style={[
            globalStyles.text,
            {
              backgroundColor: COLORS.darkGrey,
              padding: 15,
              borderRadius: 8,
              marginBottom: 20,
              width: "100%",
            },
          ]}
          placeholder="Enter your email"
          placeholderTextColor={COLORS.placeholderGrey}
          value={email}
          onChangeText={setEmail}
          keyboardType="email-address"
          autoCapitalize="none"
        />

        <Text style={[globalStyles.text, { marginBottom: 8 }]}>Password</Text>
        <TextInput
          style={[
            globalStyles.text,
            {
              backgroundColor: COLORS.darkGrey,
              padding: 15,
              borderRadius: 8,
              marginBottom: 30,
              width: "100%",
            },
          ]}
          placeholder="Enter your Password"
          placeholderTextColor={COLORS.placeholderGrey}
          value={password}
          onChangeText={setPassword}
          secureTextEntry
        />

        <TouchableOpacity
          style={[globalStyles.button, { width: "100%", padding: 15 }]}
          onPress={handleLogin}
        >
          <Text style={globalStyles.buttonText}>Log in</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

export default LoginScreen;
