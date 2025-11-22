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

  // Auth endpoints will be added here
  // async login(credentials) {
  //   // Implementation will be added later
  // },
  //
  // async register(userData) {
  //   // Implementation will be added later
  // },
};

export default ApiService;
