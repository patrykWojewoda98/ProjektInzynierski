using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Wallet.GetAllWallets
{
    public record GetWalletByClientIdQuery(int ClientId) : IRequest<WalletDto>;
}
