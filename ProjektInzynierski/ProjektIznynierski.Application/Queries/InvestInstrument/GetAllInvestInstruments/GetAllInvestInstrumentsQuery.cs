using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllInvestInstruments
{
    public class GetAllInvestInstrumentsQuery : IRequest<List<InvestInstrumentDto>>
    {
    }
}
