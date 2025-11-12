using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetAllWalletInstruments
{
    public class GetAllWalletInstrumentsQuery : IRequest<List<WalletInstrumentDto>>
    {
    }
}
