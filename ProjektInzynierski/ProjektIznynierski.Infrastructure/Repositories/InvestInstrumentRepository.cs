using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestInstrumentRepository : IInvestInstrumentRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public InvestInstrumentRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<InvestInstrument> GetByIdAsync(int id)
        {
            return await _dbContext.InvestInstruments.SingleOrDefaultAsync(ii => ii.Id == id);
        }

        public void Add(InvestInstrument investInstrument)
        {
            _dbContext.InvestInstruments.Add(investInstrument);
        }
        public void Update(InvestInstrument investInstrument)
        {
            _dbContext.InvestInstruments.Update(investInstrument);
        }

        public void Delete(InvestInstrument investInstrument)
        {
            _dbContext.InvestInstruments.Remove(investInstrument);
        }
        
    }
}
