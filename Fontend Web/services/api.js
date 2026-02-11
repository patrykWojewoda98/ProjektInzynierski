import AsyncStorage from "@react-native-async-storage/async-storage";
import api from "./apiClient";

const ApiService = {
  // =========================
  // AUTHENTICATION
  // =========================

  async login(data) {
    const response = await api.post("/auth/login", data);

    if (response.data?.token) {
      await AsyncStorage.setItem("userToken", response.data.token);
    }

    return response.data;
  },

  async loginEmployee(data) {
    const response = await api.post("/auth/login-employee", data);
    return response.data;
  },

  async verifyEmployee2FA(data) {
    const response = await api.post("/auth/verify-employee-2fa", data);

    if (response.data?.token) {
      await AsyncStorage.setItem("employeeToken", response.data.token);
    }

    return response.data;
  },

  // =========================
  // AI REQUESTS
  // =========================

  async createAnalysisRequest(investInstrumentId, clientId) {
    const response = await api.post("/AIAnalysisRequest", {
      investInstrumentId,
      clientId,
    });

    return response.data;
  },

  async getAnalysisRequestsByClientId(clientId) {
    const response = await api.get(
      `/AIAnalysisRequest/my-requests/${clientId}`,
    );

    return response.data;
  },

  async deleteAIAnalysisRequest(id) {
    await api.delete(`/AIAnalysisRequest/${id}`);
  },

  // =========================
  // AI ANALYSIS RESULT
  // =========================

  async getAIAnalysisResultById(id) {
    const response = await api.get(`/AIAnalysisResult/${id}`);
    return response.data;
  },

  // Client endpoints

  registerClient(clientData) {
    return api.post("/Client", clientData).then((r) => r.data);
  },

  getAllClients() {
    return api.get("/Client").then((r) => r.data);
  },

  getClientById(id) {
    return api.get(`/Client/${id}`).then((r) => r.data);
  },

  updateClient(id, dto) {
    return api.put(`/Client/${id}`, { id, ...dto }).then((r) => r.data);
  },

  deleteClient(id) {
    return api.delete(`/Client/${id}`);
  },

  // Country endpoints

  getAllCountries() {
    return api.get("/Country").then((r) => r.data);
  },

  getCountryById(id) {
    return api.get(`/Country/${id}`).then((r) => r.data);
  },

  getCountriesByRegion(regionId) {
    return api.get(`/Country/region/${regionId}`).then((r) => r.data);
  },

  createCountry(dto) {
    return api.post("/Country", dto).then((r) => r.data);
  },

  updateCountry(id, dto) {
    return api.put(`/Country/${id}`, { id, ...dto }).then((r) => r.data);
  },

  deleteCountry(id) {
    return api.delete(`/Country/${id}`);
  },

  // Comment endpoints

  getAllComments() {
    return api.get("/Comment").then((r) => r.data);
  },

  getCommentById(id) {
    return api.get(`/Comment/${id}`).then((r) => r.data);
  },

  getCommentsByClientId(clientId) {
    return api.get(`/Comment/client/${clientId}`).then((r) => r.data);
  },

  getCommentsByInvestInstrumentId(instrumentId) {
    return api.get(`/Comment/instrument/${instrumentId}`).then((r) => r.data);
  },

  createComment(dto) {
    return api.post("/Comment", dto).then((r) => r.data);
  },

  updateComment(id, dto) {
    return api.put(`/Comment/${id}`, { id, ...dto }).then((r) => r.data);
  },

  deleteComment(id) {
    return api.delete(`/Comment/${id}`);
  },

  // CurrencyPair endpoints

  getCurrencyPairs() {
    return api.get("/CurrencyPair").then((r) => r.data);
  },

  getCurrencyPairById(id) {
    return api.get(`/CurrencyPair/${id}`).then((r) => r.data);
  },

  createCurrencyPair(data) {
    return api.post("/CurrencyPair", data).then((r) => r.data);
  },

  updateCurrencyPair(id, data) {
    return api.put(`/CurrencyPair/${id}`, data).then((r) => r.data);
  },

  deleteCurrencyPair(id) {
    return api.delete(`/CurrencyPair/${id}`);
  },

  getCurrencyPairByCurrencies(baseCurrencyId, quoteCurrencyId) {
    return api
      .get("/CurrencyPair/by-currencies", {
        params: { baseCurrencyId, quoteCurrencyId },
      })
      .then((r) => r.data);
  },

  // CurrencyRateHistory endpoints

  getCurrencyRateHistoryById(id) {
    return api.get(`/CurrencyRateHistory/${id}`).then((r) => r.data);
  },

  getCurrencyRateHistoryByPair(currencyPairId) {
    return api
      .get(`/CurrencyRateHistory/by-pair/${currencyPairId}`)
      .then((r) => r.data);
  },

  getLatestCurrencyRate(currencyPairId) {
    return api
      .get(`/CurrencyRateHistory/latest/${currencyPairId}`)
      .then((r) => r.data);
  },

  createCurrencyRateHistory(dto) {
    return api.post("/CurrencyRateHistory", dto).then((r) => r.data);
  },

  updateCurrencyRateHistory(id, dto) {
    return api
      .put(`/CurrencyRateHistory/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteCurrencyRateHistory(id) {
    return api.delete(`/CurrencyRateHistory/${id}`);
  },

  // Employee endpoints

  getAllEmployees() {
    return api.get("/Employee").then((r) => r.data);
  },

  getEmployeeById(id) {
    return api.get(`/Employee/${id}`).then((r) => r.data);
  },

  createEmployee(dto) {
    return api.post("/Employee", dto).then((r) => r.data);
  },

  updateEmployee(id, dto) {
    return api
      .put(`/Employee/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteEmployee(id) {
    return api.delete(`/Employee/${id}`);
  },

  // PDF Generation endpoints

  openInvestmentRecommendationPdf(analysisRequestId) {
    const url = `${api.defaults.baseURL}/InvestmentRecommendationPdf/${analysisRequestId}`;
    window.location.assign(url);
  },

  // Region endpoints

  getAllRegions() {
    return api.get("/Region").then((r) => r.data);
  },

  getRegionById(id) {
    return api.get(`/Region/${id}`).then((r) => r.data);
  },

  createRegion(dto) {
    return api
      .post("/Region", {
        name: dto.name,
        regionCodeId: dto.regionCodeId,
        regionRiskLevelId: dto.regionRiskLevelId,
      })
      .then((r) => r.data);
  },

  updateRegion(id, dto) {
    return api.put(`/Region/${id}`, dto).then((r) => r.data);
  },

  deleteRegion(id) {
    return api.delete(`/Region/${id}`);
  },

  // RegionCode endpoints

  getAllRegionCodes() {
    return api.get("/RegionCode").then((r) => r.data);
  },

  getRegionCodeById(id) {
    return api.get(`/RegionCode/${id}`).then((r) => r.data);
  },

  createRegionCode(dto) {
    return api.post("/RegionCode", dto).then((r) => r.data);
  },

  updateRegionCode(id, dto) {
    return api
      .put(`/RegionCode/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteRegionCode(id) {
    return api.delete(`/RegionCode/${id}`);
  },

  // Currency endpoints

  getAllCurrencies() {
    return api.get("/Currency").then((r) => r.data);
  },

  getCurrencyByID(id) {
    return api.get(`/Currency/${id}`).then((r) => r.data);
  },

  createCurrency(dto) {
    return api
      .post("/Currency", {
        name: dto.name,
        currencyRiskLevelId: dto.currencyRiskLevelId,
      })
      .then((r) => r.data);
  },

  updateCurrency(id, dto) {
    return api.put(`/Currency/${id}`, dto).then((r) => r.data);
  },

  deleteCurrency(id) {
    return api.delete(`/Currency/${id}`);
  },

  // InvestProfile endpoints

  createInvestProfile(profileDto) {
    return api.post("/InvestProfile", profileDto).then((r) => r.data);
  },

  getInvestProfileByClientId(clientId) {
    return api
      .get(`/InvestProfile/${clientId}`)
      .then((r) => r.data)
      .catch((error) => {
        if (error.response?.status === 404) {
          return null;
        }
        throw error;
      });
  },

  updateInvestProfile(id, profileDto) {
    return api.put(`/InvestProfile/${id}`, profileDto).then((r) => r.data);
  },

  // InvestmentType endpoints

  getAllInvestmentTypes() {
    return api.get("/InvestmentType").then((r) => r.data);
  },

  getInvestmentTypeById(id) {
    return api.get(`/InvestmentType/${id}`).then((r) => r.data);
  },

  createInvestmentType(dto) {
    return api.post("/InvestmentType", dto).then((r) => r.data);
  },

  updateInvestmentType(id, dto) {
    return api
      .put(`/InvestmentType/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteInvestmentType(id) {
    return api.delete(`/InvestmentType/${id}`);
  },

  // RiskLevel endpoints

  getRiskLevels() {
    return api.get("/RiskLevel").then((r) => r.data);
  },

  getRiskLevelById(id) {
    return api.get(`/RiskLevel/${id}`).then((r) => r.data);
  },

  createRiskLevel(dto) {
    return api.post("/RiskLevel", dto).then((r) => r.data);
  },

  updateRiskLevel(id, dto) {
    return api
      .put(`/RiskLevel/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteRiskLevel(id) {
    return api.delete(`/RiskLevel/${id}`);
  },

  // MarketData endpoints

  getAllMarketData() {
    return api.get("/MarketData").then((r) => r.data);
  },

  getMarketDataById(id) {
    return api.get(`/MarketData/${id}`).then((r) => r.data);
  },

  getMarketDataByInstrumentId(instrumentId) {
    return api
      .get(`/MarketData/invest-instrument/${instrumentId}`)
      .then((r) => r.data);
  },

  createMarketData(dto) {
    return api.post("/MarketData", dto).then((r) => r.data);
  },

  updateMarketData(id, dto) {
    return api
      .put(`/MarketData/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteMarketData(id) {
    return api.delete(`/MarketData/${id}`);
  },

  importMarketDataByTicker(ticker) {
    return api.post("/MarketData/import", { ticker }).then((r) => r.data);
  },

  // InvestHorizon endpoints

  getInvestHorizons() {
    return api.get("/InvestHorizon").then((r) => r.data);
  },

  getInvestHorizonById(id) {
    return api.get(`/InvestHorizon/${id}`).then((r) => r.data);
  },

  createInvestHorizon(dto) {
    return api.post("/InvestHorizon", dto).then((r) => r.data);
  },

  updateInvestHorizon(id, dto) {
    return api
      .put(`/InvestHorizon/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteInvestHorizon(id) {
    return api.delete(`/InvestHorizon/${id}`);
  },

  // FinancialMetric endpoints

  getFinancialMetricById(id) {
    return api.get(`/FinancialMetric/${id}`).then((r) => r.data);
  },

  createFinancialMetricForInstrument(instrumentId, dto) {
    return api
      .post(`/InvestInstrument/${instrumentId}/FinancialMetric`, dto)
      .then((r) => r.data);
  },

  updateFinancialMetric(id, dto) {
    return api
      .put(`/FinancialMetric/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteFinancialMetric(id) {
    return api.delete(`/FinancialMetric/${id}`);
  },

  importFinancialMetric(dto) {
    return api.post("/FinancialMetric/import", dto).then((r) => r.data);
  },

  // FinancialReport endpoints

  getFinancialReportsByInvestInstrumentId(instrumentId) {
    return api
      .get(`/FinancialReport/invest-instrument/${instrumentId}`)
      .then((r) => r.data);
  },

  getFinancialReports() {
    return api.get("/FinancialReport").then((r) => r.data);
  },

  getFinancialReportById(id) {
    return api.get(`/FinancialReport/${id}`).then((r) => r.data);
  },

  createFinancialReport(dto) {
    return api.post("/FinancialReport", dto).then((r) => r.data);
  },

  updateFinancialReport(id, dto) {
    return api.put(`/FinancialReport/${id}`, dto).then((r) => r.data);
  },

  deleteFinancialReport(id) {
    return api.delete(`/FinancialReport/${id}`);
  },

  importFinancialReportsByIsin(isin) {
    return api.post("/FinancialReport/import", { isin }).then((r) => r.data);
  },

  // Sector endpoints

  getSectors() {
    return api.get("/Sector").then((r) => r.data);
  },

  getSectorById(id) {
    return api.get(`/Sector/${id}`).then((r) => r.data);
  },

  createSector(dto) {
    return api.post("/Sector", dto).then((r) => r.data);
  },

  updateSector(id, dto) {
    return api
      .put(`/Sector/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteSector(id) {
    return api.delete(`/Sector/${id}`);
  },

  // InvestInstrument endpoints

  getInvestInstruments() {
    return api.get("/InvestInstrument").then((r) => r.data);
  },

  getInvestInstrumentById(id) {
    return api.get(`/InvestInstrument/${id}`).then((r) => r.data);
  },

  getInvestInstrumentsByRegion(regionId) {
    return api.get(`/InvestInstrument/region/${regionId}`).then((r) => r.data);
  },

  getInvestInstrumentsByType(typeId) {
    return api.get(`/InvestInstrument/type/${typeId}`).then((r) => r.data);
  },

  getInvestInstrumentsBySector(sectorId) {
    return api.get(`/InvestInstrument/sector/${sectorId}`).then((r) => r.data);
  },

  createInvestInstrument(dto) {
    return api.post("/InvestInstrument", dto).then((r) => r.data);
  },

  updateInvestInstrument(id, dto) {
    return api
      .put(`/InvestInstrument/${id}`, {
        id,
        ...dto,
      })
      .then((r) => r.data);
  },

  deleteInvestInstrument(id) {
    return api.delete(`/InvestInstrument/${id}`);
  },

  // WatchListItem endpoints

  getWatchListItemsByClientId(clientId) {
    return api.get(`/WatchListItem/client/${clientId}`).then((r) => r.data);
  },

  addWatchListItem(clientId, investInstrumentId) {
    return api
      .post("/WatchListItem", {
        clientId,
        investInstrumentId,
      })
      .then((r) => r.data);
  },

  deleteWatchListItem(id) {
    return api.delete(`/WatchListItem/${id}`);
  },

  // Wallet endpoints

  getWalletByClientId(clientId) {
    return api.get(`/Wallet/client/${clientId}`).then((r) => r.data);
  },

  getWalletInstrumentsByWalletId(walletId) {
    return api.get(`/WalletInstrument/wallet/${walletId}`).then((r) => r.data);
  },

  addWalletInstrument(walletId, investInstrumentId, quantity) {
    return api
      .post("/WalletInstrument", {
        walletId,
        investInstrumentId,
        quantity,
      })
      .then((r) => r.data);
  },

  getWalletInvestmentSummary(walletId) {
    return api
      .get(`/WalletInstrument/${walletId}/investments-summary`)
      .then((r) => r.data);
  },

  getWalletInstrumentById(id) {
    return api.get(`/WalletInstrument/${id}`).then((r) => r.data);
  },

  updateWalletInstrument(id, data) {
    return api.put(`/WalletInstrument/${id}`, data).then((r) => r.data);
  },

  deleteWalletInstrument(id) {
    return api.delete(`/WalletInstrument/${id}`);
  },

  updateWallet(walletId, data) {
    return api
      .put(`/Wallet/${walletId}`, {
        id: walletId,
        ...data,
      })
      .then((res) => res.data);
  },
  // Excel export endpoints

  openWalletExcelExport(walletId) {
    const url = `${api.defaults.baseURL}/Wallet/${walletId}/export`;
    window.location.assign(url);
  },
};

export default ApiService;
