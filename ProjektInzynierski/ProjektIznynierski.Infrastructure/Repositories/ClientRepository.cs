using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class ClientRepository : IClientRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public ClientRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _dbContext.Clients.SingleOrDefaultAsync(c => c.Id == id);
        }

        public Task<Client?> GetByEmailAsync(string email)
        {
            return _dbContext.Clients.SingleOrDefaultAsync(c => c.Email == email);
        }

        public Task<bool> CheckIfClientExist(string email)
        {
            return _dbContext.Clients.AnyAsync(c => c.Email == email);
        }

        public void Add(Client client)
        {
            _dbContext.Clients.Add(client);
        }
        public void Update(Client client)
        {
            _dbContext.Clients.Update(client);
        }

        public void Delete(Client client)
        {
            _dbContext.Clients.Remove(client);
        }
    }
}
