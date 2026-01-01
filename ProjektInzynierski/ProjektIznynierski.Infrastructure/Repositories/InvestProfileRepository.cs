using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestProfileRepository : GenericRepository<InvestProfile>, IInvestProfileRepository
    {
        public InvestProfileRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<InvestProfile?> GetByClientIdAsync(int clientId,CancellationToken cancellationToken = default)
        {
            return await _dbContext.InvestProfiles.FirstOrDefaultAsync(ip => ip.ClientId == clientId, cancellationToken);
        }

    }
}
