using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class FinancialMetricRepository : IFinancialMetricRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public FinancialMetricRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<FinancialMetric> GetByIdAsync(int id)
        {
            return await _dbContext.FinancialMetrics.SingleOrDefaultAsync(fm => fm.Id == id);
        }
        public void Add(FinancialMetric financialMetric)
        {
            _dbContext.FinancialMetrics.Add(financialMetric);
        }
        public void Update(FinancialMetric financialMetric)
        {
            _dbContext.FinancialMetrics.Update(financialMetric);
        }
        public void Delete(FinancialMetric financialMetric)
        {
            _dbContext.FinancialMetrics.Remove(financialMetric);
        }
        
    }
}
