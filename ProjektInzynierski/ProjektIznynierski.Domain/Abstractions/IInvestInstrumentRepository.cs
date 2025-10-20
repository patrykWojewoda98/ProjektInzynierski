using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IInvestInstrumentRepository
    {
        Task<InvestInstrument> GetByIdAsync(int id);
        void Add(InvestInstrument investInstrument);
        void Update(InvestInstrument investInstrument);
        void Delete(InvestInstrument investInstrument);
    }
}
