import AsyncStorage from "@react-native-async-storage/async-storage";
import axios from "axios";

const API_BASE_URL = "http://10.0.2.2:5036/api";

const ApiService = {
  // Authentication endpoints
  async login(data) {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error("Invalid credentials");
    }

    const json = await response.json();

    // Zapis tokenu do AsyncStorage
    if (json?.token) {
      await AsyncStorage.setItem("userToken", json.token);
    }

    return json;
  },
  // Register endpoint
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

  //Currency endpoints
  async getCurrencyByID(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Currency/${id}`);
      return response.data;
    } catch (error) {
      if (error.response?.status === 404) {
        throw ["Client not found. Please check your ID."];
      }
      console.error("Error fetching client:", error);
      throw ["An error occurred while fetching client data. Please try again."];
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

  async getInvestProfileByClientId(clientId) {
    try {
      const response = await axios.get(
        `${API_BASE_URL}/InvestProfile/${clientId}`
      );
      return response.data;
    } catch (error) {
      if (error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  async updateInvestProfile(id, profileDto) {
    const response = await axios.put(
      `${API_BASE_URL}/InvestProfile/${id}`,
      profileDto
    );
    return response.data;
  },

  // RiskLevel endpoints
  async getRiskLevels() {
    try {
      const response = await axios.get(`${API_BASE_URL}/RiskLevel`);
      return response.data;
    } catch (error) {
      console.error("Error fetching risk levels:", error);
      throw ["Failed to load risk levels"];
    }
  },

  // InvestHorizon endpoints
  async getInvestHorizons() {
    try {
      const response = await axios.get(`${API_BASE_URL}/InvestHorizon`);
      return response.data;
    } catch (error) {
      console.error("Error fetching invest horizons:", error);
      throw error;
    }
  },

  //FinancialMetric endpoints
  async getFinancialMetricById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/FinancialMetric/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching financial metric:", error);
      throw error;
    }
  },

  // FinancialReport endpoints
  async getFinancialReportsByInvestInstrumentId(instrumentId) {
    try {
      const response = await axios.get(
        `${API_BASE_URL}/FinancialReport/invest-instrument/${instrumentId}`
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching financial reports:", error);
      throw error;
    }
  },

  //Sector endpoints
  async getSectors() {
    try {
      const response = await axios.get(`${API_BASE_URL}/Sector`);
      return response.data;
    } catch (error) {
      console.error("Error fetching sectors:", error);
      throw error;
    }
  },

  // InvestInstrument endpoints
  async getInvestInstruments() {
    const res = await axios.get(`${API_BASE_URL}/InvestInstrument`);
    return res.data;
  },

  async getInvestInstrumentById(id) {
    const res = await axios.get(`${API_BASE_URL}/InvestInstrument/${id}`);
    return res.data;
  },

  async getInvestInstrumentsByRegion(regionId) {
    const res = await axios.get(
      `${API_BASE_URL}/InvestInstrument/region/${regionId}`
    );
    return res.data;
  },

  async getInvestInstrumentsByType(typeId) {
    const res = await axios.get(
      `${API_BASE_URL}/InvestInstrument/type/${typeId}`
    );
    return res.data;
  },

  async getInvestInstrumentsBySector(sectorId) {
    const res = await axios.get(
      `${API_BASE_URL}/InvestInstrument/sector/${sectorId}`
    );
    return res.data;
  },

  // WatchListItem endpoints
  async getWatchListItemsByClientId(clientId) {
    const response = await axios.get(
      `${API_BASE_URL}/WatchListItem/client/${clientId}`
    );
    return response.data;
  },

  async addWatchListItem(clientId, investInstrumentId) {
    const response = await axios.post(`${API_BASE_URL}/WatchListItem`, {
      clientId,
      investInstrumentId,
    });
    return response.data;
  },
  async deleteWatchListItem(id) {
    await axios.delete(`${API_BASE_URL}/WatchListItem/${id}`);
  },

  // Wallet endpoints
  async getWalletByClientId(clientId) {
    const res = await axios.get(`${API_BASE_URL}/Wallet/client/${clientId}`);
    return res.data;
  },

  async getWalletInstrumentsByWalletId(walletId) {
    const res = await axios.get(
      `${API_BASE_URL}/WalletInstrument/wallet/${walletId}`
    );
    return res.data;
  },

  async addWalletInstrument(walletId, investInstrumentId, quantity) {
    const response = await axios.post(`${API_BASE_URL}/WalletInstrument`, {
      walletId,
      investInstrumentId,
      quantity,
    });
    return response.data;
  },
  async getWalletInvestmentSummary(walletId) {
    const res = await axios.get(
      `${API_BASE_URL}/WalletInstrument/${walletId}/investments-summary`
    );
    return res.data;
  },
};

export default ApiService;
