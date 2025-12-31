export const ROUTES = {
  MAIN_MENU: "/mainMenu",
  COMING_SOON: "/ComingSoon",
  LOGIN: "/auth/login",
  REGISTER: "/auth/register",
  INVEST_PROFILE: "/investProfile/investProfile",
  WATCHLIST: "/watchList/watchList",
  WALLET: "/wallet/wallet",
  TRADE_HISTORY: "/tradeHistory",
  MARKET_DATA: "/marketData",
  INVEST_INSTRUMENT: "/investInstrument/investInstrument",
  FINANCIAL_REPORT: "/financialReport",
  FINANCIAL_METRIC: "/financialMetric/financialMetric",
  FINANCIAL_METRIC_PREVIEW: "/financialMetric/financialMetricPreview",
  FINANCIAL_REPORT_BY_INSTRUMENT:
    "/financialReport/financialReportsByInstrument",
  CREATE_FINANCIAL_REPORT_BY_AI: "/financialReport/createFinancialReportByAI",
} as const;
