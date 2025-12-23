using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetAllByRegionId;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllInvestInstruments
{
    public class GetAllInvestmentTypesQueryHandler : IRequestHandler<GetAllInvestmentTypesQuery, List<InvestmentTypeDto>>
    {
        private readonly IInvestmentTypeRepository _investmentTypeRepository;

        public GetAllInvestmentTypesQueryHandler(IInvestmentTypeRepository investmentTypeRepository)
        {
            _investmentTypeRepository = investmentTypeRepository;
        }

        public async Task<List<InvestmentTypeDto>> Handle(GetAllInvestmentTypesQuery request, CancellationToken cancellationToken)
        {
            var investmentTypes = await _investmentTypeRepository.GetAllAsync(cancellationToken);

            return investmentTypes.Select(i => new InvestmentTypeDto
            {
                Id = i.Id,
                TypeName = i.TypeName
            }).ToList();
        }
    }
}
