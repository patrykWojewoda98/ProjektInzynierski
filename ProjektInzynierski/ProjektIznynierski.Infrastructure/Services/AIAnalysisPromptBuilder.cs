using ProjektIznynierski.Application.Services;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Repositories;
using System.Text;

namespace ProjektIznynierski.Infrastructure.Services
{
    internal class AIAnalysisPromptBuilder : IAIAnalysisPromptBuilder
    {
        private readonly IClientRepository _clientRepository;
        private readonly IInvestProfileRepository _investProfileRepository;
        private readonly IInvestHorizonRepository _investHorizonRepository;
        private readonly IInvestInstrumentRepository _investInstrumentRepository;
        private readonly IFinancialMetricRepository _financialMetricRepository;
        private readonly IInvestmentTypeRepository _investmentTypeRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IRiskLevelRepository _riskLevelRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ISectorRepository _sectorRepository;
        private readonly IFinancialReportRepository _financialReportRepository;
        private readonly IMarketDataRepository _marketDataRepository;

        public AIAnalysisPromptBuilder(IClientRepository clientRepository, IInvestProfileRepository investProfileRepository, IInvestHorizonRepository investHorizonRepository, IInvestInstrumentRepository investInstrumentRepository,
        IFinancialMetricRepository financialMetricRepository, IInvestmentTypeRepository investmentTypeRepository, IRegionRepository regionRepository, IRiskLevelRepository riskLevelRepository, ICountryRepository countryRepository,
        ICurrencyRepository currencyRepository, ISectorRepository sectorRepository, IFinancialReportRepository financialReportRepository, IMarketDataRepository marketDataRepository)
        {
            _clientRepository = clientRepository;
            _investProfileRepository = investProfileRepository;
            _investHorizonRepository = investHorizonRepository;
            _investInstrumentRepository = investInstrumentRepository;
            _financialMetricRepository = financialMetricRepository;
            _investmentTypeRepository = investmentTypeRepository;
            _regionRepository = regionRepository;
            _riskLevelRepository = riskLevelRepository;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _sectorRepository = sectorRepository;
            _financialReportRepository = financialReportRepository;
            _marketDataRepository = marketDataRepository;
        }

        public async Task<string> BuildPromptAsync(
    int clientId,
    int investInstrumentId,
    CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();

            sb.AppendLine("""
SYSTEM:
You are an advanced AI financial advisor and senior investment analyst.

Your task is to perform a strict, data-driven investment analysis based solely on the provided information.
You must prioritize capital preservation, risk management, and alignment with the client’s investment profile.

Analyze all provided data holistically, including:
- client risk tolerance and investment horizon,
- financial metrics,
- macro, regional, country, currency, and sector risks,
- historical financial reports,
- latest market data.

Act conservatively:
- NEVER recommend an investment that exceeds the client’s maximum acceptable risk level.
- If data is insufficient, inconsistent, or signals elevated risk, prefer HOLD or SELL over BUY.
- Do not assume missing data.

Your recommendation must be logical, explainable, and defensible from a professional financial advisory perspective.

Return ONLY valid JSON strictly matching the schema provided at the end.
Do not include any additional text, explanations, or formatting outside the JSON.
""");

            // ================= CLIENT =================
            var client = await _clientRepository
                .GetByIdAsync(clientId, cancellationToken);

            if (client == null)
                throw new Exception("Client not found");

            var investProfile = await _investProfileRepository
                .GetByClientIdAsync(client.Id, cancellationToken);

            if (investProfile == null)
                throw new Exception("InvestProfile not found for client");

            InvestHorizon? investHorizon = null;
            if (investProfile.InvestHorizonId.HasValue)
            {
                investHorizon = await _investHorizonRepository
                    .GetByIdAsync(investProfile.InvestHorizonId.Value, cancellationToken);
            }

            var acceptableRiskLevel = await _riskLevelRepository.GetByIdAsync(investProfile.AcceptableRiskLevelId, cancellationToken);

            // ================= INSTRUMENT =================
            var instrument = await _investInstrumentRepository
                .GetByIdAsync(investInstrumentId, cancellationToken);

            if (instrument == null)
                throw new Exception("InvestInstrument not found");

            // Financial Metrics
            FinancialMetric? metrics = null;
            if (instrument.FinancialMetricId.HasValue)
            {
                metrics = await _financialMetricRepository
                    .GetByIdAsync(instrument.FinancialMetricId.Value, cancellationToken);
            }

            // Investment Type
            var investmentType = await _investmentTypeRepository
                .GetByIdAsync(instrument.InvestmentTypeId, cancellationToken);

            // Region + Risk
            var region = await _regionRepository
                .GetByIdAsync(instrument.RegionId, cancellationToken);

            var regionRisk = await _riskLevelRepository
                .GetByIdAsync(region.RegionRiskLevelId, cancellationToken);

            // Country + Risk
            var country = await _countryRepository
                .GetByIdAsync(instrument.CountryId, cancellationToken);

            var countryRisk = await _riskLevelRepository
                .GetByIdAsync(country.CountryRiskLevelId, cancellationToken);

            // Currency + Risk
            var currency = await _currencyRepository
                .GetByIdAsync(instrument.CurrencyId, cancellationToken);

            var currencyRisk = await _riskLevelRepository
                .GetByIdAsync(currency.CurrencyRiskLevelId, cancellationToken);

            // Sector
            var sector = await _sectorRepository
                .GetByIdAsync(instrument.SectorId, cancellationToken);

            var maxRiskLevelScale = await _riskLevelRepository.GetMaxRiskLevel();


            //Finiacaial reports
            var reports = await _financialReportRepository.GetByInstrumentIdAsync(investInstrumentId, cancellationToken);

            //Market data
            var marketData = await _marketDataRepository.GetLatestByInvestInstrumentIdAsync(investInstrumentId, cancellationToken);

            // ================= PROMPT CONTENT =================

            sb.AppendLine("\n=== CLIENT PROFILE ===");
            sb.AppendLine($"Profile: {investProfile.ProfileName}");
            sb.AppendLine($"Target return: {investProfile.TargetReturn}%");
            sb.AppendLine($"Max acceptable risk level: {acceptableRiskLevel.RiskLevelScale} " +$"({acceptableRiskLevel.Description}). " +$"If exceeded, reduce confidence score accordingly.");
            sb.AppendLine($"Max drawdown: {investProfile.MaxDrawDown}%");
            sb.AppendLine($"Investment horizon: {investHorizon?.Horizon ?? "Not specified"}");

            sb.AppendLine("\n=== INVESTMENT INSTRUMENT ===");
            sb.AppendLine($"Name: {instrument.Name}");
            sb.AppendLine($"Ticker: {instrument.Ticker}");
            sb.AppendLine($"Description: {instrument.Description}");
            sb.AppendLine($"Type: {investmentType?.TypeName}");

            sb.AppendLine("\n=== FINANCIAL METRICS ===");
            if (metrics != null)
            {
                sb.AppendLine($"PE: {metrics.PE}");
                sb.AppendLine($"PB: {metrics.PB}");
                sb.AppendLine($"ROE: {metrics.ROE}");
                sb.AppendLine($"Debt to Equity: {metrics.DebtToEquity}");
                sb.AppendLine($"Dividend Yield: {metrics.DividendYield}");
            }
            else
            {
                sb.AppendLine("No financial metrics available");
            }

            sb.AppendLine("\n=== REGION ===");
            sb.AppendLine($"Region: {region.Name}");
            sb.AppendLine($"Region risk level: {regionRisk.Description} (scale {regionRisk.RiskLevelScale}/{maxRiskLevelScale.RiskLevelScale}");

            sb.AppendLine("\n=== COUNTRY ===");
            sb.AppendLine($"Country: {country.Name}");
            sb.AppendLine($"Country risk level: {countryRisk.Description} (scale {countryRisk.RiskLevelScale})/{maxRiskLevelScale.RiskLevelScale}");

            sb.AppendLine("\n=== CURRENCY ===");
            sb.AppendLine($"Currency: {currency.Name}");
            sb.AppendLine($"Currency risk level: {currencyRisk.Description} (scale {currencyRisk.RiskLevelScale}/{maxRiskLevelScale.RiskLevelScale})");

            sb.AppendLine("\n=== SECTOR ===");
            sb.AppendLine($"Sector: {sector.Name}");
            sb.AppendLine($"Description: {sector.Description}");

            sb.AppendLine("\n=== FINANCIAL REPORTS ===");

            if (reports.Any())
            {
                foreach (var r in reports)
                {
                    sb.AppendLine($"""
Period: {r.Period}
Revenue: {r.Revenue}
Net income: {r.NetIncome}
EPS: {r.EPS}
Assets: {r.Assets}
Liabilities: {r.Liabilities}
Operating cash flow: {r.OperatingCashFlow}
Free cash flow: {r.FreeCashFlow}
--- 
""");
                }
            }
            else
            {
                sb.AppendLine("No financial reports available");
            }

            sb.AppendLine("\n=== MARKET DATA (LATEST) ===");

            if (marketData != null)
            {
                sb.AppendLine($"""
Date: {marketData.Date:yyyy-MM-dd}
Open price: {marketData.OpenPrice}
Close price: {marketData.ClosePrice}
High price: {marketData.HighPrice}
Low price: {marketData.LowPrice}
Volume: {marketData.Volume}
Daily change: {marketData.DailyChange}
Daily change %: {marketData.DailyChangePercent}
""");
            }
            else
            {
                sb.AppendLine("No market data available.");
            }

            sb.AppendLine("""
=== RESPONSE FORMAT (JSON ONLY) ===
{
  "summary": "string",
  "recommendation": "BUY | HOLD | SELL",
  "keyInsights": "string",
  "confidenceScore": number,
  "clientId": number
}
""");

            return sb.ToString();
        }
    }
}

