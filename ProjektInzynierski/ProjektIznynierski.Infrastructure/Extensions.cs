using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjektInzynierski.Application.Interfaces;
using ProjektInzynierski.Infrastructure.Services;
using ProjektIznynierski.Application.Services.Sources;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Infrastructure.Context;
using ProjektIznynierski.Infrastructure.Repositories;
using ProjektIznynierski.Infrastructure.Services;

namespace ProjektIznynierski.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IAIAnalisisRequestRepository, AIAnalisisRequestRepository>();
            services.AddScoped<IAIAnalysisResultRepository, AIAnalysisResultRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IInvestProfileRepository, InvestProfileRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IFinancialMetricRepository, FinancialMetricRepository>();
            services.AddScoped<IInvestInstrumentRepository, InvestInstrumentRepository>();
            services.AddScoped<IFinancialReportRepository, FinancialReportRepository>();
            services.AddScoped<IMarketDataRepository, MarketDataRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletInstrumentRepository, WalletInstrumentRepository>();
            services.AddScoped<IWatchListItemRepository, WatchListItemRepository>();
            services.AddScoped<IWatchListRepository, WatchListRepository>();
            services.AddScoped<IInvestHorizonRepository, InvestHorizonRepository>();
            services.AddScoped<IRiskLevelRepository, RiskLevelRepository>();
            services.AddScoped<IInvestmentTypeRepository, InvestmentTypeRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRegionCodeRepository, RegionCodeRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICurrencyPairRepository, CurrencyPairRepository>();
            services.AddScoped<ICurrencyRateHistoryRepository, CurrencyRateHistoryRepository>();

            services.AddScoped<IJwtTokenService, JwtService>();
            services.AddHttpClient<IChatGPTService, ChatGPTService>();
            services.AddScoped<IStrefaInwestorowClientService, StrefaInwestorowClientService>();   
            services.AddScoped<IAIAnalysisPromptBuilder, AIAnalysisPromptBuilder>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITwoFactorCodeService, TwoFactorCodeService>();
            services.AddScoped<IYahooFinanceService, YahooFinanceService>();
            services.AddScoped<IInvestmentRecommendationPdfService, InvestmentRecommendationPdfService>();


            services.AddDbContext<ProjektInzynierskiDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ProjektInzynierskiCS");
                options.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
