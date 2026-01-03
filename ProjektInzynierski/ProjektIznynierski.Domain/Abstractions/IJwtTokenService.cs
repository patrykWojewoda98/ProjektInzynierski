using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(Client client);
        Task<string> GenerateEployeeToken(Employee employee);

    }
}
