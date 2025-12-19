using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestHorizon.GetAllInvestHorizons
{
    public class GetAllInvestHorizonsQueryHandler : IRequestHandler<GetAllInvestHorizonsQuery, List<InvestHorizonDto>>
    {
        private readonly IInvestHorizonRepository _investHorizonRepository;
        public GetAllInvestHorizonsQueryHandler(IInvestHorizonRepository investHorizonRepository)
        {
            _investHorizonRepository = investHorizonRepository;
        }
        public async Task<List<InvestHorizonDto>>   Handle(GetAllInvestHorizonsQuery request, CancellationToken cancellationToken)
        {
            var investHorizons = await _investHorizonRepository.GetAllAsync(cancellationToken);

            return investHorizons.Select(fr => new InvestHorizonDto
            {
                Id = fr.Id,
                Horizon = fr.Horizon
            }).ToList();
        }
    }
}
