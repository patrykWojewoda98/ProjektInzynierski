using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestmentType.GetInvestmentTypeById
{
    public class GetInvestmentTypeByIdQueryHandler
        : IRequestHandler<GetInvestmentTypeByIdQuery, InvestmentTypeDto>
    {
        private readonly IInvestmentTypeRepository _repository;

        public GetInvestmentTypeByIdQueryHandler(IInvestmentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<InvestmentTypeDto> Handle(
            GetInvestmentTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var investmentType =
                await _repository.GetByIdAsync(request.id, cancellationToken);

            if (investmentType is null)
                throw new Exception($"InvestmentType with id {request.id} not found.");

            return new InvestmentTypeDto
            {
                Id = investmentType.Id,
                TypeName = investmentType.TypeName
            };
        }
    }
}
