using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.UpdateCurrencyPair
{
    internal class UpdateCurrencyPairCommandHandler : IRequestHandler<UpdateCurrencyPairCommand, CurrencyPairDto>
    {
        private readonly ICurrencyPairRepository _currencyPairRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCurrencyPairCommand> _validator;

        public UpdateCurrencyPairCommandHandler(ICurrencyPairRepository currencyPairRepository,IUnitOfWork unitOfWork,IValidator<UpdateCurrencyPairCommand> validator)
        {
            _currencyPairRepository = currencyPairRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CurrencyPairDto> Handle(UpdateCurrencyPairCommand request,CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(
                    $"Validation failed: {string.Join(", ", errors)}");
            }

            var entity = await _currencyPairRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                throw new Exception(
                    $"Currency pair with id {request.Id} not found.");

            entity.BaseCurrencyId = request.BaseCurrencyId;
            entity.QuoteCurrencyId = request.QuoteCurrencyId;
            entity.Symbol = request.Symbol;

            _currencyPairRepository.Update(entity);
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
