using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IFinancialReportRepository
    {
        Task<FinancialReport> GetByIdAsync(int id);
        void Add(FinancialReport entity);
        void Update(FinancialReport entity);
        void Delete(FinancialReport entity);
    }
}
