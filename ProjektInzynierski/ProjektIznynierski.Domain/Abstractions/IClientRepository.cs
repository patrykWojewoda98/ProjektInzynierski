using ProjektIznynierski.Domain.Entities;


namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int id);
        Task<bool> CheckIfClientExist(string email);
        Task<Client?> GetByEmailAsync(string email);
        void Add(Client client);
        void Update(Client client);
        void Delete(Client client);
    }
}
