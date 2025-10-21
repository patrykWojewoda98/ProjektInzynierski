using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Client?> GetByEmailAsync(string email)
        {
            return await _dbContext.Clients.SingleOrDefaultAsync(c => c.Email == email);
        }

        public async Task<bool> CheckIfClientExist(string email)
        {
            return await _dbContext.Clients.AnyAsync(c => c.Email == email);
        }

        public async Task<Wallet?> GetClientWalletAsync(int clientId)
        {
            return await _dbContext.Wallets.SingleOrDefaultAsync(w => w.ClientId == clientId);
        }
    }
}
