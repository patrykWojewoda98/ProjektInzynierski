using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Wallet.GetWalletById
{
    public record GetWalletByIdQuery(int id) : IRequest<WalletDto>
    {
    }
}
