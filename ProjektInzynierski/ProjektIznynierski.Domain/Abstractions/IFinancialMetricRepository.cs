using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IFinancialMetricRepository
    {
        Task<FinancialMetric> GetByIdAsync(int id);
        void Add(FinancialMetric financialMetric);
        void Update(FinancialMetric financialMetric);
        void Delete(FinancialMetric financialMetric);
    }
}
