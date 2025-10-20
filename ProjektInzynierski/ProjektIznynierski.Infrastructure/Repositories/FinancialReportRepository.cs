using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;


namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class FinancialReportRepository : IFinancialReportRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public FinancialReportRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FinancialReport> GetByIdAsync(int id)
        {
            return await _dbContext.FinancialReports.SingleOrDefaultAsync(fr => fr.Id == id);
        }

        public void Add(FinancialReport entity)
        {
            _dbContext.FinancialReports.Add(entity);
        }
        public void Update(FinancialReport entity)
        {
            _dbContext.FinancialReports.Add(entity);
        }

        public void Delete(FinancialReport entity)
        {
            _dbContext.FinancialReports.Remove(entity);
        }
    }
}
