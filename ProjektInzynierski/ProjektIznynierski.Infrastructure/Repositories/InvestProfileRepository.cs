using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestProfileRepository : IInvestProfileRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public InvestProfileRepository(ProjektInzynierskiDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<InvestProfile> GetInvestProfileByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.InvestProfiles.SingleOrDefaultAsync(ip => ip.Id == id, cancellationToken);
        }

        public void Add(InvestProfile entity)
        {
            _dbContext.InvestProfiles.Add(entity);
        }
        public void Update(InvestProfile entity)
        {
            _dbContext.InvestProfiles.Update(entity);
        }
        public void Delete(InvestProfile entity)
        {
            _dbContext.InvestProfiles.Remove(entity);
        }

        

        
    }
}
