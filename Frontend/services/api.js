import axios from "axios";

const API_BASE_URL = "http://10.0.2.2:5036/api";

const ApiService = {
  // Region endpoints
  async getRegions() {
    try {
      const response = await axios.get(`${API_BASE_URL}/region`);
      return response.data;
    } catch (error) {
      console.error("Error fetching regions:", error);
      throw error;
    }
  },

  async getCountriesByRegion(regionId) {
    try {
      const response = await axios.get(
        `${API_BASE_URL}/Country/region/${regionId}`
      );
      const data = response.data;

      return Array.isArray(data) ? data : [data];
    } catch (error) {
      console.error("Error fetching countries:", error);
      throw error;
    }
  },

  // Client endpoints
  async getClientById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Client/${id}`);
      return response.data;
    } catch (error) {
      if (error.response?.status === 404) {
        throw ["Client not found. Please check your ID."];
      }
      console.error("Error fetching client:", error);
      throw ["An error occurred while fetching client data. Please try again."];
    }
  },

  async registerClient(clientData) {
    try {
      const response = await axios.post(`${API_BASE_URL}/Client`, clientData);
      return response.data;
    } catch (error) {
      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }
      console.error("Registration error:", error);
      throw ["An error occurred during registration. Please try again."];
    }
  },

  // InvestProfile endpoints
  async createInvestProfile(profileDto) {
    try {
      const response = await axios.post(
        `${API_BASE_URL}/InvestProfile`,
        profileDto
      );
      return response.data;
    } catch (error) {
      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }
      console.error("Error creating investment profile:", error);
      throw ["An error occurred while creating the investment profile."];
    }
  },
};

export default ApiService;
