using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestInstrumentRepository : GenericRepository<InvestInstrument>, IInvestInstrumentRepository
    {
        public InvestInstrumentRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }
        
    }
}
