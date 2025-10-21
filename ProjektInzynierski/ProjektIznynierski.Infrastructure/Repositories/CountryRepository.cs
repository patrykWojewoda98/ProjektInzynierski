using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        
        public CountryRepository(ProjektInzynierskiDbContext dbContext) : base (dbContext)
        {
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _dbContext.Countries.ToListAsync();
        }
    }
}
