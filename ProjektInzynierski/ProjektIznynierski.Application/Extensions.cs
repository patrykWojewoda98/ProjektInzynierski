using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProjektIznynierski.Application.Commands.Client.AddClient;
using ProjektIznynierski.Application.Commands.Client.UpdateClient;
using ProjektIznynierski.Application.Commands.Country.CreateCountry;
using ProjektIznynierski.Application.Commands.Country.UpdateCountry;
using ProjektIznynierski.Application.Commands.Currency.CreateCurrency;
using ProjektIznynierski.Application.Commands.Currency.UpdateCurrency;
using ProjektIznynierski.Application.Commands.Region.CreateRegion;
using ProjektIznynierski.Application.Commands.Region.UpdateRegion;
using ProjektIznynierski.Application.Commands.Sector.CreateSector;
using ProjektIznynierski.Application.Commands.Sector.UpdateSector;
using ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric;
using ProjektIznynierski.Application.Commands.FinancialMetric.UpdateFinancialMetric;
using ProjektIznynierski.Application.Commands.FinancialReport.CreateFinancialReport;
using ProjektIznynierski.Application.Commands.FinancialReport.UpdateFinancialReport;
using ProjektIznynierski.Application.Commands.InvestInstrument.CreateInvestInstrument;
using ProjektIznynierski.Application.Commands.InvestInstrument.UpdateInvestInstrument;
using ProjektIznynierski.Application.Commands.MarketData.CreateMarketData;
using ProjektIznynierski.Application.Commands.MarketData.UpdateMarketData;
using ProjektIznynierski.Application.Commands.Wallet.CreateWallet;
using ProjektIznynierski.Application.Commands.Wallet.UpdateWallet;
using ProjektIznynierski.Application.Commands.WalletInstrument.CreateWalletInstrument;
using ProjektIznynierski.Application.Commands.WalletInstrument.UpdateWalletInstrument;
using ProjektIznynierski.Application.Commands.TradeHistory.CreateTradeHistory;
using ProjektIznynierski.Application.Commands.TradeHistory.UpdateTradeHistory;
using ProjektIznynierski.Application.Commands.InvestProfile.CreateInvestProfile;
using ProjektIznynierski.Application.Commands.InvestProfile.UpdateInvestProfile;
using ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest;
using ProjektIznynierski.Application.Commands.WatchList.CreateWatchList;
using ProjektIznynierski.Application.Commands.WatchList.UpdateWatchList;
using ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem;
using ProjektIznynierski.Application.Commands.WatchListItem.UpdateWatchListItem;
using System.Reflection;
using ProjektIznynierski.Application.Commands.InvestHorizon.AddInvestHorizon;
using ProjektIznynierski.Application.Commands.InvestHorizon.UpdateInvestHorizon;
using ProjektIznynierski.Application.Commands.RiskLevel.CreateRiskLevel;
using ProjektIznynierski.Application.Commands.RiskLevel.UpdateRiskLevel;
using MediatR;
using ProjektIznynierski.Application.Commands.Employee.AddEmployee;
using ProjektIznynierski.Application.Commands.Employee.UpdateEmployee;
using ProjektIznynierski.Application.Commands.RegionCode.CreateRegionCode;
using ProjektIznynierski.Application.Commands.RegionCode.UpdateRegionCode;
using ProjektIznynierski.Application.Commands.FinancialMetric.ImportFinancialIndicators;

namespace ProjektIznynierski.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddClientCommand>, AddClientCommandValidation>();
            services.AddScoped<IValidator<UpdateClientCommand>, UpdateClientCommandValidation>();

            services.AddScoped<IValidator<CreateCountryCommand>, CreateCountryCommandValidation>();
            services.AddScoped<IValidator<UpdateCountryCommand>, UpdateCountryCommandValidation>();

            services.AddScoped<IValidator<CreateCurrencyCommand>, CreateCurrencyCommandValidation>();
            services.AddScoped<IValidator<UpdateCurrencyCommand>, UpdateCurrencyCommandValidation>();

            services.AddScoped<IValidator<CreateRegionCommand>, CreateRegionCommandValidation>();
            services.AddScoped<IValidator<UpdateRegionCommand>, UpdateRegionCommandValidation>();

            services.AddScoped<IValidator<CreateSectorCommand>, CreateSectorCommandValidation>();
            services.AddScoped<IValidator<UpdateSectorCommand>, UpdateSectorCommandValidation>();

            services.AddScoped<IValidator<CreateFinancialMetricAndAssignToInstrumentCommand>, CreateFinancialMetricAndAssignToInstrumentCommandValidation>();
            services.AddScoped<IValidator<UpdateFinancialMetricCommand>, UpdateFinancialMetricCommandValidation>();

            services.AddScoped<IValidator<CreateFinancialReportCommand>, CreateFinancialReportCommandValidation>();
            services.AddScoped<IValidator<UpdateFinancialReportCommand>, UpdateFinancialReportCommandValidation>();

            services.AddScoped<IValidator<CreateInvestInstrumentCommand>, CreateInvestInstrumentCommandValidation>();
            services.AddScoped<IValidator<UpdateInvestInstrumentCommand>, UpdateInvestInstrumentCommandValidation>();

            services.AddScoped<IValidator<CreateMarketDataCommand>, CreateMarketDataCommandValidation>();
            services.AddScoped<IValidator<UpdateMarketDataCommand>, UpdateMarketDataCommandValidation>();

            services.AddScoped<IValidator<CreateWalletCommand>, CreateWalletCommandValidation>();
            services.AddScoped<IValidator<UpdateWalletCommand>, UpdateWalletCommandValidation>();

            services.AddScoped<IValidator<CreateWalletInstrumentCommand>, CreateWalletInstrumentCommandValidation>();
            services.AddScoped<IValidator<UpdateWalletInstrumentCommand>, UpdateWalletInstrumentCommandValidation>();

            services.AddScoped<IValidator<CreateTradeHistoryCommand>, CreateTradeHistoryCommandValidation>();
            services.AddScoped<IValidator<UpdateTradeHistoryCommand>, UpdateTradeHistoryCommandValidation>();

            services.AddScoped<IValidator<CreateInvestProfileCommand>, CreateInvestProfileCommandValidation>();
            services.AddScoped<IValidator<UpdateInvestProfileCommand>, UpdateInvestProfileCommandValidation>();

            services.AddScoped<IValidator<CreateAIAnalysisRequestCommand>, CreateAIAnalysisRequestCommandValidation>();


            services.AddScoped<IValidator<CreateWatchListCommand>, CreateWatchListCommandValidation>();
            services.AddScoped<IValidator<UpdateWatchListCommand>, UpdateWatchListCommandValidation>();

            services.AddScoped<IValidator<CreateWatchListItemCommand>, CreateWatchListItemCommandValidation>();
            services.AddScoped<IValidator<UpdateWatchListItemCommand>, UpdateWatchListItemCommandValidation>();

            services.AddScoped<IValidator<AddInvestHorizonCommand>, AddInvestHorizonCommandValidation>();
            services.AddScoped<IValidator<UpdateInvestHorizonCommand>, UpdateInvestHorizonCommandValidation>();

            services.AddScoped<IValidator<CreateRiskLevelCommand>, CreateRiskLevelCommandValidation>();
            services.AddScoped<IValidator<UpdateRiskLevelCommand>, UpdateRiskLevelCommandValidation>();

            services.AddScoped<IValidator<CreateInvestmentTypeCommand>, CreateInvestmentTypeCommandValidation>();
            services.AddScoped<IValidator<UpdateInvestmentTypeCommand>, UpdateInvestmentTypeCommandValidation>();

            services.AddScoped<IValidator<AddEmployeeCommand>, AddEmployeeCommandValidation>();
            services.AddScoped<IValidator<UpdateEmployeeCommand>, UpdateEmployeeCommandValidation>();

            services.AddScoped<IValidator<CreateRegionCodeCommand>, CreateRegionCodeCommandValidator>();
            services.AddScoped<IValidator<UpdateRegionCodeCommand>, UpdateRegionCodeCommandValidator>();

            services.AddScoped<IValidator<ImportFinancialMetricCommand>, ImportFinancialMetricCommandValidator>();


            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
