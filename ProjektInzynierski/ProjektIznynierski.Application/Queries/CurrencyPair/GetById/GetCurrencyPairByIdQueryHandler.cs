using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyPair.GetById
{
    internal class GetCurrencyPairByIdQueryHandler
        : IRequestHandler<GetCurrencyPairByIdQuery, CurrencyPairDto>
    {
        private readonly ICurrencyPairRepository _currencyPairRepository;

        public GetCurrencyPairByIdQueryHandler(ICurrencyPairRepository currencyPairRepository)
        {
            _currencyPairRepository = currencyPairRepository;
        }

        public async Task<CurrencyPairDto> Handle(GetCurrencyPairByIdQuery request,CancellationToken cancellationToken)
        {
            var pair = await _currencyPairRepository
                .GetByIdAsync(request.Id, cancellationToken);

            if (pair == null)
            {
                throw new Exception(
                    $"Currency pair with ID {request.Id} not found."
                );
            }

            return new CurrencyPairDto
            {
                Id = pair.Id,
                BaseCurrencyId = pair.BaseCurrencyId,
                QuoteCurrencyId = pair.QuoteCurrencyId,
                Symbol = pair.Symbol
            };
        }
    }
}
