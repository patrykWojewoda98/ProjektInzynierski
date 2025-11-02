using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentById
{
    public record GetWalletInstrumentByIdQuery(int id) : IRequest<WalletInstrumentDto>
    {
    }
}
