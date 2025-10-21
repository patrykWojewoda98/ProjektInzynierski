using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Infrastructure.Context;
using ProjektIznynierski.Infrastructure.Repositories;

namespace ProjektIznynierski.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddScoped<ITradeHistoryRepository, TradeHistoryRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletInstrumentRepository, WalletInstrumentRepository>();


            services.AddDbContext<ProjektInzynierskiDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ProjektInzynierskiDatabase");
                options.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
