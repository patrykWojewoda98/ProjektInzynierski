using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICountryRepository
    {
        Task<Country> GetByIdAsync(int id);
        Task<List<Country>> GetAllAsync();
        void Add(Country country);
        void Update(Country country);
        void Delete(Country country);

    }
}
