using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.AddCurrencyPair
{
    internal class AddCurrencyPairCommandHandler: IRequestHandler<AddCurrencyPairCommand, CurrencyPairDto>
    {
        private readonly ICurrencyPairRepository _currencyPairRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCurrencyPairCommand> _validator;

        public AddCurrencyPairCommandHandler(ICurrencyPairRepository currencyPairRepository,IUnitOfWork unitOfWork,IValidator<AddCurrencyPairCommand> validator)
        {
            _currencyPairRepository = currencyPairRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CurrencyPairDto> Handle(AddCurrencyPairCommand request,CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(
                    $"Validation failed: {string.Join(", ", errors)}");
            }

            var existingPair = await _currencyPairRepository.GetByCurrenciesAsync(request.BaseCurrencyId,request.QuoteCurrencyId);

            if (existingPair != null)
                throw new Exception("Currency pair already exists.");

            var entity = new Domain.Entities.CurrencyPair
            {
                BaseCurrencyId = request.BaseCurrencyId,
                QuoteCurrencyId = request.QuoteCurrencyId,
                Symbol = request.Symbol
            };

            _currencyPairRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CurrencyPairDto
            {
                Id = entity.Id,
                BaseCurrencyId = entity.BaseCurrencyId,
                QuoteCurrencyId = entity.QuoteCurrencyId,
                Symbol = entity.Symbol
            };
        }
    }
}
