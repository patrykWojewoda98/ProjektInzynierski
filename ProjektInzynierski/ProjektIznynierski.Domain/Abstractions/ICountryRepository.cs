using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<List<Country>> GetAllAsync();

    }
}
