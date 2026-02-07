import AsyncStorage from "@react-native-async-storage/async-storage";
import axios from "axios";
import Constants from "expo-constants";
import { Platform } from "react-native";

const getApiBaseUrl = () => {
  if (Platform.OS === "web") {
    return "http://localhost:5036/api";
  }

  if (Platform.OS === "android") {
    return "http://10.0.2.2:5036/api";
  }

  const host = Constants.expoConfig?.hostUri?.split(":")[0];

  if (!host) {
    return "http://localhost:5036/api";
  }

  return `http://${host}:5036/api`;
};

const API_BASE_URL = getApiBaseUrl();

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

  async loginEmployee(data) {
    const response = await fetch(`${API_BASE_URL}/auth/login-employee`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    const json = await response.json();

    if (!response.ok) {
      throw new Error(json?.message || "Login failed");
    }
    return json;
  },

  async verifyEmployee2FA(data) {
    const response = await fetch(`${API_BASE_URL}/auth/verify-employee-2fa`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    const json = await response.json();

    if (!response.ok) {
      throw new Error(json?.message || "Invalid verification code");
    }

    if (json?.token) {
      await AsyncStorage.setItem("employeeToken", json.token);
    }

    return json;
  },

  //AI Requests endpoints
  async createAnalysisRequest(investInstrumentId, clientId) {
    try {
      const response = await axios.post(`${API_BASE_URL}/AIAnalysisRequest`, {
        investInstrumentId,
        clientId,
      });
      return response.data;
    } catch (error) {
      console.error("Error creating analysis request:", error);
      throw ["An error occurred while creating the analysis request."];
    }
  },
  async getAnalysisRequestsByClientId(clientId) {
    try {
      const response = await axios.get(
        `${API_BASE_URL}/AIAnalysisRequest/my-requests/${clientId}`
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching analysis requests:", error);
      throw ["An error occurred while fetching analysis requests."];
    }
  },
  async deleteAIAnalysisRequest(id) {
    try {
      await axios.delete(`${API_BASE_URL}/AIAnalysisRequest/${id}`);
    } catch (error) {
      console.error("Error deleting analysis request:", error);
      throw ["An error occurred while deleting the analysis request."];
    }
  },

  //AI Analysis Result endpoints
  async getAIAnalysisResultById(id) {
    const res = await axios.get(`${API_BASE_URL}/AIAnalysisResult/${id}`);
    return res.data;
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

async getAllClients() {
  try {
    const response = await axios.get(`${API_BASE_URL}/Client`);
    return response.data;
  } catch (error) {
    console.error("Error fetching clients:", error);
    throw ["Failed to load clients."];
  }
},

async getClientById(id) {
  try {
    const response = await axios.get(`${API_BASE_URL}/Client/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching client:", error);
    throw ["Failed to load client."];
  }
},

async updateClient(id, dto) {
  try {
    const response = await axios.put(`${API_BASE_URL}/Client/${id}`, {
      id,
      ...dto,
    });
    return response.data;
  } catch (error) {
    if (error.response?.data?.errors) {
      throw error.response.data.errors;
    }
    console.error("Error updating client:", error);
    throw ["Failed to update client."];
  }
},

async deleteClient(id) {
  try {
    await axios.delete(`${API_BASE_URL}/Client/${id}`);
  } catch (error) {
    console.error("Error deleting client:", error);
    throw ["Failed to delete client."];
  }
},

  //Country endpoints
  async getAllCountries() {
    try {
      const response = await axios.get(`${API_BASE_URL}/Country`);
      return response.data;
    } catch (error) {
      console.error("Error fetching countries:", error);
      throw ["Failed to load countries."];
    }
  },

  async getCountryById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Country/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching country:", error);
      throw ["Failed to load country."];
    }
  },
  async getCountriesByRegion(regionId) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Country/region/${regionId}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching countries by region:", error);
      throw ["Failed to load countries for the selected region."];
    }
  },

  async createCountry(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/Country`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating country:", error);
      throw ["Failed to create country."];
    }
  },

  async updateCountry(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/Country/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating country:", error);
      throw ["Failed to update country."];
    }
  },

  async deleteCountry(id) {
    try {
      await axios.delete(`${API_BASE_URL}/Country/${id}`);
    } catch (error) {
      console.error("Error deleting country:", error);
      throw ["Failed to delete country."];
    }
  },

  // Comment endpoints
async getAllComments() {
  try {
    const response = await axios.get(`${API_BASE_URL}/Comment`);
    return response.data;
  } catch (error) {
    console.error("Error fetching comments:", error);
    throw ["Failed to load comments."];
  }
},

async getCommentById(id) {
  try {
    const response = await axios.get(`${API_BASE_URL}/Comment/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching comment:", error);
    throw ["Failed to load comment."];
  }
},

async getCommentsByClientId(clientId) {
  try {
    const response = await axios.get(
      `${API_BASE_URL}/Comment/client/${clientId}`
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching client comments:", error);
    throw ["Failed to load client comments."];
  }
},

async getCommentsByInvestInstrumentId(instrumentId) {
  try {
    const response = await axios.get(
      `${API_BASE_URL}/Comment/instrument/${instrumentId}`
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching instrument comments:", error);
    throw ["Failed to load comments for this instrument."];
  }
},

async createComment(dto) {
  try {
    const response = await axios.post(`${API_BASE_URL}/Comment`, dto);
    return response.data;
  } catch (error) {
    console.error("Error creating comment:", error);
    throw ["Failed to add comment."];
  }
},

async updateComment(id, dto) {
  try {
    const response = await axios.put(`${API_BASE_URL}/Comment/${id}`, {
      id,
      ...dto,
    });
    return response.data;
  } catch (error) {
    console.error("Error updating comment:", error);
    throw ["Failed to update comment."];
  }
},

async deleteComment(id) {
  try {
    await axios.delete(`${API_BASE_URL}/Comment/${id}`);
  } catch (error) {
    console.error("Error deleting comment:", error);
    throw ["Failed to delete comment."];
  }
},
// CurrencyPair endpoints
async getCurrencyPairs() {
  const res = await axios.get(`${API_BASE_URL}/CurrencyPair`);
  return res.data;
},

async getCurrencyPairById(id) {
  const res = await axios.get(`${API_BASE_URL}/CurrencyPair/${id}`);
  return res.data;
},

async createCurrencyPair(data) {
  const res = await axios.post(`${API_BASE_URL}/CurrencyPair`, data);
  return res.data;
},

async updateCurrencyPair(id, data) {
  const res = await axios.put(`${API_BASE_URL}/CurrencyPair/${id}`, data);
  return res.data;
},

async deleteCurrencyPair(id: number) {
  const res = await axios.delete(`${API_BASE_URL}/CurrencyPair/${id}`);
  return res.data;
},

async getCurrencyPairByCurrencies(baseCurrencyId, quoteCurrencyId) {
  const res = await axios.get(
    `${API_BASE_URL}/CurrencyPair/by-currencies`,
    {
      params: { baseCurrencyId, quoteCurrencyId },
    }
  );
  return res.data;
},

// CurrencyRateHistory endpoints
async getCurrencyRateHistoryById(id: number) {
  try {
    const response = await axios.get(
      `${API_BASE_URL}/CurrencyRateHistory/${id}`,
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching currency rate history by id:", error);
    throw ["Failed to load currency rate history."];
  }
},
async getCurrencyRateHistoryByPair(currencyPairId: number) {
  try {
    const response = await axios.get(
      `${API_BASE_URL}/CurrencyRateHistory/by-pair/${currencyPairId}`,
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching currency rate history:", error);
    throw ["Failed to load currency rate history."];
  }
},

async getLatestCurrencyRate(currencyPairId: number) {
  try {
    const response = await axios.get(
      `${API_BASE_URL}/CurrencyRateHistory/latest/${currencyPairId}`,
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching latest currency rate:", error);
    throw ["Failed to load latest currency rate."];
  }
},

async createCurrencyRateHistory(dto: any) {
  try {
    const response = await axios.post(
      `${API_BASE_URL}/CurrencyRateHistory`,
      dto,
    );
    return response.data;
  } catch (error) {
    console.error("Error creating currency rate history:", error);
    throw ["Failed to create currency rate history."];
  }
},

async updateCurrencyRateHistory(id: number, dto: any) {
  try {
    const response = await axios.put(
      `${API_BASE_URL}/CurrencyRateHistory/${id}`,
      {
        id,
        ...dto,
      },
    );
    return response.data;
  } catch (error) {
    console.error("Error updating currency rate history:", error);
    throw ["Failed to update currency rate history."];
  }
},

async deleteCurrencyRateHistory(id: number) {
  try {
    await axios.delete(`${API_BASE_URL}/CurrencyRateHistory/${id}`);
  } catch (error) {
    console.error("Error deleting currency rate history:", error);
    throw ["Failed to delete currency rate history."];
  }
},

  //Employee endpoints
  async getAllEmployees() {
    try {
      const response = await axios.get(`${API_BASE_URL}/Employee`);
      return response.data;
    } catch (error) {
      console.error("Error fetching employees:", error);
      throw ["Failed to load employees."];
    }
  },

  async getEmployeeById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Employee/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching employee:", error);
      throw ["Failed to load employee."];
    }
  },

  async createEmployee(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/Employee`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating employee:", error);
      throw ["Failed to create employee."];
    }
  },

  async updateEmployee(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/Employee/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating employee:", error);
      throw ["Failed to update employee."];
    }
  },

  async deleteEmployee(id) {
    try {
      await axios.delete(`${API_BASE_URL}/Employee/${id}`);
    } catch (error) {
      console.error("Error deleting employee:", error);
      throw ["Failed to delete employee."];
    }
  },

  // PDF Generation endpoints
openInvestmentRecommendationPdf(analysisRequestId: number) {
  const url = `${API_BASE_URL}/InvestmentRecommendationPdf/${analysisRequestId}`;

  // omija Expo Router i SPA – prawdziwy request HTTP
  window.location.assign(url);
},

  // Region endpoints
  async getAllRegions() {
    try {
      const response = await axios.get(`${API_BASE_URL}/Region`);
      return response.data;
    } catch (error) {
      console.error("Error fetching regions:", error);
      throw ["An error occurred while fetching regions."];
    }
  },

  async getRegionById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Region/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching region:", error);
      throw ["An error occurred while fetching region."];
    }
  },

  async createRegion(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/Region`, {
        name: dto.name,
        regionCodeId: dto.regionCodeId, // może być null
        regionRiskLevelId: dto.regionRiskLevelId, // wymagane
      });

      return response.data;
    } catch (error) {
      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }

      console.error("Error creating region:", error);
      throw ["An error occurred while creating region."];
    }
  },
  async updateRegion(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/Region/${id}`, dto);
      return response.data;
    } catch (error) {
      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }
      console.error("Error updating region:", error);
      throw ["An error occurred while updating region."];
    }
  },

  async deleteRegion(id) {
    try {
      await axios.delete(`${API_BASE_URL}/Region/${id}`);
    } catch (error) {
      console.error("Error deleting region:", error);
      throw ["An error occurred while deleting region."];
    }
  },

  // RegionCode endpoints
  async getAllRegionCodes() {
    const res = await axios.get(`${API_BASE_URL}/RegionCode`);
    return res.data;
  },

  async getRegionCodeById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/RegionCode/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching region code:", error);
      throw ["Failed to fetch region code."];
    }
  },

  async createRegionCode(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/RegionCode`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating region code:", error);
      throw ["Failed to create region code."];
    }
  },

  async updateRegionCode(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/RegionCode/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating region code:", error);
      throw ["Failed to update region code."];
    }
  },

  async deleteRegionCode(id) {
    try {
      await axios.delete(`${API_BASE_URL}/RegionCode/${id}`);
    } catch (error) {
      console.error("Error deleting region code:", error);
      throw ["Failed to delete region code."];
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
  async getAllCurrencies() {
    try {
      const response = await axios.get(`${API_BASE_URL}/Currency`);
      return response.data;
    } catch (error) {
      console.error("Error fetching currencies:", error);
      throw ["An error occurred while fetching currencies."];
    }
  },
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

  async createCurrency(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/Currency`, {
        name: dto.name,
        currencyRiskLevelId: dto.currencyRiskLevelId,
      });

      return response.data;
    } catch (error) {
      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }

      console.error("Error creating currency:", error);
      throw ["An error occurred while creating currency."];
    }
  },

  async updateCurrency(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/Currency/${id}`, dto);
      return response.data;
    } catch (error) {
      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }
      console.error("Error updating currency:", error);
      throw ["An error occurred while updating currency."];
    }
  },

  async deleteCurrency(id) {
    try {
      await axios.delete(`${API_BASE_URL}/Currency/${id}`);
    } catch (error) {
      console.error("Error deleting currency:", error);
      throw ["An error occurred while deleting currency."];
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

  // InvestmentType endpoints
  async getAllInvestmentTypes() {
    try {
      const res = await axios.get(`${API_BASE_URL}/InvestmentType`);
      return res.data;
    } catch (e) {
      console.error("Error fetching investment types:", e);
      throw ["Failed to load investment types."];
    }
  },

  async getInvestmentTypeById(id) {
    try {
      const res = await axios.get(`${API_BASE_URL}/InvestmentType/${id}`);
      return res.data;
    } catch {
      throw ["Failed to load investment type."];
    }
  },

  async createInvestmentType(dto) {
    try {
      const res = await axios.post(`${API_BASE_URL}/InvestmentType`, dto);
      return res.data;
    } catch {
      throw ["Failed to create investment type."];
    }
  },

  async updateInvestmentType(id, dto) {
    try {
      const res = await axios.put(`${API_BASE_URL}/InvestmentType/${id}`, {
        id,
        ...dto,
      });
      return res.data;
    } catch {
      throw ["Failed to update investment type."];
    }
  },

  async deleteInvestmentType(id) {
    try {
      await axios.delete(`${API_BASE_URL}/InvestmentType/${id}`);
    } catch {
      throw ["Failed to delete investment type."];
    }
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
  async getRiskLevelById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/RiskLevel/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching risk level:", error);
      throw ["Failed to load risk level."];
    }
  },

  async createRiskLevel(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/RiskLevel`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating risk level:", error);
      throw ["Failed to create risk level."];
    }
  },

  async updateRiskLevel(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/RiskLevel/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating risk level:", error);
      throw ["Failed to update risk level."];
    }
  },

  async deleteRiskLevel(id) {
    try {
      await axios.delete(`${API_BASE_URL}/RiskLevel/${id}`);
    } catch (error) {
      console.error("Error deleting risk level:", error);
      throw ["Failed to delete risk level."];
    }
  },

  //MarketData endpoints
  async getAllMarketData() {
    try {
      const response = await axios.get(`${API_BASE_URL}/MarketData`);
      return response.data;
    } catch (error) {
      console.error("Error fetching market data:", error);
      throw ["Failed to fetch market data."];
    }
  },

  async getMarketDataById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/MarketData/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching market data by id:", error);
      throw ["Failed to fetch market data."];
    }
  },

  async getMarketDataByInstrumentId(instrumentId) {
    try {
      const response = await axios.get(
        `${API_BASE_URL}/MarketData/invest-instrument/${instrumentId}`
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching market data for instrument:", error);
      throw ["Failed to fetch market data for selected instrument."];
    }
  },

  async createMarketData(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/MarketData`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating market data:", error);
      throw ["Failed to create market data."];
    }
  },

  async updateMarketData(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/MarketData/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating market data:", error);
      throw ["Failed to update market data."];
    }
  },

  async deleteMarketData(id) {
    try {
      await axios.delete(`${API_BASE_URL}/MarketData/${id}`);
    } catch (error) {
      console.error("Error deleting market data:", error);
      throw ["Failed to delete market data."];
    }
  },

  async importMarketDataByTicker(ticker) {
    try {
      const response = await axios.post(`${API_BASE_URL}/MarketData/import`, {
        ticker,
      });
      return response.data;
    } catch (error) {
      console.error("Error importing market data:", error);

      if (error.response?.data?.errors) {
        throw error.response.data.errors;
      }

      throw ["Failed to import market data for given ticker."];
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

  async getInvestHorizonById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/InvestHorizon/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching invest horizon:", error);
      throw ["Failed to load investment horizon."];
    }
  },

  async createInvestHorizon(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/InvestHorizon`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating invest horizon:", error);
      throw ["Failed to create investment horizon."];
    }
  },

  async updateInvestHorizon(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/InvestHorizon/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating invest horizon:", error);
      throw ["Failed to update investment horizon."];
    }
  },

  async deleteInvestHorizon(id) {
    try {
      await axios.delete(`${API_BASE_URL}/InvestHorizon/${id}`);
    } catch (error) {
      console.error("Error deleting invest horizon:", error);
      throw ["Failed to delete investment horizon."];
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
  async createFinancialMetricForInstrument(instrumentId, dto) {
    try {
      const response = await axios.post(
        `${API_BASE_URL}/InvestInstrument/${instrumentId}/FinancialMetric`,
        dto
      );
      return response.data;
    } catch (error) {
      console.error("Error creating financial metric:", error);
      throw error;
    }
  },
  async updateFinancialMetric(id, dto) {
    try {
      const response = await axios.put(
        `${API_BASE_URL}/FinancialMetric/${id}`,
        {
          id,
          ...dto,
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error updating financial metric:", error);
      throw error;
    }
  },
  async deleteFinancialMetric(id) {
    try {
      await axios.delete(`${API_BASE_URL}/FinancialMetric/${id}`);
    } catch (error) {
      console.error("Error deleting financial metric:", error);
      throw error;
    }
  },

  async importFinancialMetric(dto) {
  try {
    const response = await axios.post(
      `${API_BASE_URL}/FinancialMetric/import`,
      dto
    );
    return response.data; // zwraca metricId
  } catch (error) {
    console.error(
      "Error importing financial metric automatically:",
      error
    );
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

  getFinancialReports() {
    return axios.get(`${API_BASE_URL}/FinancialReport`).then((r) => r.data);
  },

  getFinancialReportById(id) {
    return axios
      .get(`${API_BASE_URL}/FinancialReport/${id}`)
      .then((res) => res.data);
  },
  createFinancialReport(dto) {
    return axios.post(`${API_BASE_URL}/FinancialReport`, dto);
  },
  updateFinancialReport(id, dto) {
    return axios.put(`${API_BASE_URL}/FinancialReport/${id}`, dto);
  },

  deleteFinancialReport(id) {
    return axios.delete(`${API_BASE_URL}/FinancialReport/${id}`);
  },

  async importFinancialReportsByIsin(isin: string) {
    try {
      const response = await axios.post(
        `${API_BASE_URL}/FinancialReport/import`,
        { isin }
      );
      return response.data;
    } catch (error) {
      console.error("Error importing financial reports:", error);
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
  async getSectorById(id) {
    try {
      const response = await axios.get(`${API_BASE_URL}/Sector/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching sector:", error);
      throw ["Failed to load sector."];
    }
  },

  async createSector(dto) {
    try {
      const response = await axios.post(`${API_BASE_URL}/Sector`, dto);
      return response.data;
    } catch (error) {
      console.error("Error creating sector:", error);
      throw ["Failed to create sector."];
    }
  },

  async updateSector(id, dto) {
    try {
      const response = await axios.put(`${API_BASE_URL}/Sector/${id}`, {
        id,
        ...dto,
      });
      return response.data;
    } catch (error) {
      console.error("Error updating sector:", error);
      throw ["Failed to update sector."];
    }
  },

  async deleteSector(id) {
    try {
      await axios.delete(`${API_BASE_URL}/Sector/${id}`);
    } catch (error) {
      console.error("Error deleting sector:", error);
      throw ["Failed to delete sector."];
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

  async createInvestInstrument(dto) {
    try {
      const response = await axios.post(
        `${API_BASE_URL}/InvestInstrument`,
        dto
      );
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  async updateInvestInstrument(id, dto) {
    try {
      const response = await axios.put(
        `${API_BASE_URL}/InvestInstrument/${id}`,
        {
          id,
          ...dto,
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error updating instrument:", error);
      throw ["Failed to update investment instrument."];
    }
  },

  async deleteInvestInstrument(id) {
    await axios.delete(`${API_BASE_URL}/InvestInstrument/${id}`);
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
getWalletInstrumentById(id: number) {
  return axios
    .get(`${API_BASE_URL}/WalletInstrument/${id}`)
    .then((res) => res.data);
},
updateWalletInstrument(id: number, data: any) {
  return axios.put(
    `${API_BASE_URL}/WalletInstrument/${id}`,
    data,
  );
},

async deleteWalletInstrument(id: number) {
  await axios.delete(`${API_BASE_URL}/WalletInstrument/${id}`);
},

  async updateWallet(walletId, data) {
  const res = await axios.put(
    `${API_BASE_URL}/Wallet/${walletId}`,
    data
  );
  return res.data;
},

// Excel export endpoints
openWalletExcelExport(walletId: number) {
  const url = `${API_BASE_URL}/Wallet/${walletId}/export`;

  // pełny redirect do backendu (poza Expo Router)
  window.location.assign(url);
},
};

export default ApiService;