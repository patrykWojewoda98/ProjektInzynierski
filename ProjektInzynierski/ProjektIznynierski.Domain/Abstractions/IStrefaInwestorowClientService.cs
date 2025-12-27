using ProjektInzynierski.Domain.Models;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IStrefaInwestorowClientService
    {
        Task<List<FinancialReportSnapshot>> GetFinancialReportsAsync(string isin, CancellationToken ct);
    }
}
