using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestHorizon.GetInvestHorizonByID
{
    public class GetInvestHorizonByIdQueryHandler : IRequestHandler<GetInvestHorizonByIdQuery, InvestHorizonDto>
    {
        private readonly IInvestHorizonRepository _investHorizonRepository;

        public GetInvestHorizonByIdQueryHandler(IInvestHorizonRepository investHorizonRepository)
        {
            _investHorizonRepository = investHorizonRepository;
        }
        public async Task<InvestHorizonDto> Handle(GetInvestHorizonByIdQuery request, CancellationToken cancellationToken)
        {
            var investHorizon = await _investHorizonRepository.GetByIdAsync(request.id, cancellationToken);

            if (investHorizon is null)
            {
                throw new Exception($"InvestHorizon with id {request.id} not found.");
            }

            var investHorizonDto = new InvestHorizonDto
            {
                Id = investHorizon.Id,
                Horizon = investHorizon.Horizon
            };
            return investHorizonDto;
        }
    }
}
