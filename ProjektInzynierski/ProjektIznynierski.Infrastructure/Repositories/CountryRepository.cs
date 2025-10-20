using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CountryRepository : ICountryRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public CountryRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Country> GetByIdAsync(int id)
        {
            return await _dbContext.Countries.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _dbContext.Countries.ToListAsync();
        }

        public void Add(Country country)
        {
            _dbContext.Countries.Add(country);
        }
        public void Update(Country country)
        {
            _dbContext.Countries.Update(country);
        }

        public void Delete(Country country)
        {
            _dbContext.Countries.Remove(country);
        }
    }
}
