using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById
{
    internal class GetInvestProfileByIdQueryHandler : IRequestHandler<GetInvestProfileByIdQuery, InvestProfileDto>
    {
        private readonly IInvestProfileRepository _investProfileRepository;
        public GetInvestProfileByIdQueryHandler(IInvestProfileRepository investProfileRepository)
        {
            _investProfileRepository = investProfileRepository;
        }

        public async Task<InvestProfileDto> Handle(GetInvestProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _investProfileRepository.GetByIdAsync(request.id, cancellationToken);
            if (profile is null)
            {
                throw new Exception($"InvestProfile with id {request.id} not found.");
            }

            return new InvestProfileDto
            {
                Id = profile.Id,
                ProfileName = profile.ProfileName,
                AcceptableRisk = (int)profile.AcceptableRisk,
                InvestHorizonId = profile.InvestHorizonId,
                TargetReturn = profile.TargetReturn,
                MaxDrawDown = profile.MaxDrawDown,
                ClientId = profile.ClientId
            };
        }
    }
}
