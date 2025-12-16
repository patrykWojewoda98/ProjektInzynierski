using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles
{
    public class GetAllInvestProfilesQueryHandler : IRequestHandler<GetAllInvestProfilesQuery, List<InvestProfileDto>>
    {
        private readonly IInvestProfileRepository _investProfileRepository;

        public GetAllInvestProfilesQueryHandler(IInvestProfileRepository investProfileRepository)
        {
            _investProfileRepository = investProfileRepository;
        }

        public async Task<List<InvestProfileDto>> Handle(GetAllInvestProfilesQuery request, CancellationToken cancellationToken)
        {
            var investProfiles = await _investProfileRepository.GetAllAsync(cancellationToken);
            
            return investProfiles.Select(ip => new InvestProfileDto
            {
                Id = ip.Id,
                ProfileName = ip.ProfileName,
                AcceptableRiskLevelId = ip.AcceptableRiskLevelId,
                InvestHorizonId = ip.InvestHorizonId,
                TargetReturn = ip.TargetReturn,
                MaxDrawDown = ip.MaxDrawDown,
                ClientId = ip.ClientId
            }).ToList();
        }
    }
}
