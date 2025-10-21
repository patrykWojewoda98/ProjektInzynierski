using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;


namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class FinancialReportRepository : GenericRepository<FinancialReport>, IFinancialReportRepository
    {
        public FinancialReportRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }
    }
}
