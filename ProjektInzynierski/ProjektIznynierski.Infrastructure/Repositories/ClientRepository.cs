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

        public Task<Client?> GetByEmailAsync(string email)
        {
            return _dbContext.Clients.SingleOrDefaultAsync(c => c.Email == email);
        }

        public Task<bool> CheckIfClientExist(string email)
        {
            return _dbContext.Clients.AnyAsync(c => c.Email == email);
        }

    }
}
