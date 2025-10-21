using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class FinancialMetricRepository : GenericRepository<FinancialMetric>, IFinancialMetricRepository
    {
        public FinancialMetricRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
