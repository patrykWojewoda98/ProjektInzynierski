using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Currency.GetCurrencyById
{
    internal class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, CurrencyDto>
    {
        private readonly ICurrencyRepository _repository;
        public GetCurrencyByIdQueryHandler(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<CurrencyDto> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Currency with id {request.id} not found.");
            }

            return new CurrencyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CurrencyRiskLevelId = entity.CurrencyRiskLevelId,
            };
        }
    }
}
