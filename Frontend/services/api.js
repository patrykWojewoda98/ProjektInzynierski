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

      // Jeśli API zwraca obiekt zamiast tablicy, opakuj go w tablicę:
      return Array.isArray(data) ? data : [data];
    } catch (error) {
      console.error("Error fetching countries:", error);
      throw error;
    }
  },

  // Client endpoints
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
};

export default ApiService;
