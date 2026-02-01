using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.AddCurrencyRateHistory
{
    internal class AddCurrencyRateHistoryCommandHandler : IRequestHandler<AddCurrencyRateHistoryCommand, CurrencyRateHistoryDto>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;
        private readonly ICurrencyPairRepository _currencyPairRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCurrencyRateHistoryCommand> _validator;

        public AddCurrencyRateHistoryCommandHandler(ICurrencyRateHistoryRepository rateHistoryRepository,ICurrencyPairRepository currencyPairRepository,IUnitOfWork unitOfWork, IValidator<AddCurrencyRateHistoryCommand> validator)
        {
            _rateHistoryRepository = rateHistoryRepository;
            _currencyPairRepository = currencyPairRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CurrencyRateHistoryDto> Handle(AddCurrencyRateHistoryCommand request,CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(
                    $"Validation failed: {string.Join(", ", errors)}");
            }

            var pairExists = await _currencyPairRepository
                .GetByIdAsync(request.CurrencyPairId, cancellationToken);

            if (pairExists == null)
                throw new Exception("Currency pair does not exist.");

            var entity = new Domain.Entities.CurrencyRateHistory
            {
                CurrencyPairId = request.CurrencyPairId,
                Date = request.Date,
                CloseRate = request.CloseRate,
                OpenRate = request.OpenRate,
                HighRate = request.HighRate,
                LowRate = request.LowRate
            };

            _rateHistoryRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CurrencyRateHistoryDto
            {
                Id = entity.Id,
                CurrencyPairId = entity.CurrencyPairId,
                Date = entity.Date,
                CloseRate = entity.CloseRate,
                OpenRate = entity.OpenRate,
                HighRate = entity.HighRate,
                LowRate = entity.LowRate
            };
        }
    }
}
