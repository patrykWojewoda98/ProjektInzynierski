using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentById
{
    public record GetWalletInstrumentsByWalletIdQuery(int WalletId) : IRequest<List<WalletInstrumentDto>>;
}
