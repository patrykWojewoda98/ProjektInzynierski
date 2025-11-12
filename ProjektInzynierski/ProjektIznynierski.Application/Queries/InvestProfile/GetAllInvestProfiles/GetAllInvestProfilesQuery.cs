using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles
{
    public class GetAllInvestProfilesQuery : IRequest<List<InvestProfileDto>>
    {
    }
}
