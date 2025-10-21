using ProjektIznynierski.Domain.Entities;


namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<bool> CheckIfClientExist(string email);
        Task<Client?> GetByEmailAsync(string email);

        Task<Wallet?> GetClientWalletAsync(int clientId);
    }
}
