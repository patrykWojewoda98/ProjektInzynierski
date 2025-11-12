using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.Wallet.GetAllWallets
{
    public class GetAllWalletsQuery : IRequest<List<WalletDto>>
    {
    }
}
