using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<bool> CheckAuthorityByPeselAsync(string pesel);
        Task<bool> CheckIfEmployeeExistsAsync(string email);
        Task<Employee?> GetByEmailAsync(string email);
    }
}
