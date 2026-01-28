using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByClientIdAsync(int clientId);
        Task<IEnumerable<Comment>> GetByInvestInstrumentIdAsync(int investInstrumentId);
    }
}
