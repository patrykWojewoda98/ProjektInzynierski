using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class EmployeeRepository: GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ProjektInzynierskiDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<bool> CheckAuthorityByPeselAsync(string pesel)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.Pesel == pesel);
        }

        public async Task<bool> CheckIfEmployeeExistsAsync(string email)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.Email == email);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
