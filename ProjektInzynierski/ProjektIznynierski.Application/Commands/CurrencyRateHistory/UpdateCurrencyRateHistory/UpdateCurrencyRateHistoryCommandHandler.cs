using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.UpdateCurrencyRateHistory
{
    internal class UpdateCurrencyRateHistoryCommandHandler : IRequestHandler<UpdateCurrencyRateHistoryCommand, CurrencyRateHistoryDto>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCurrencyRateHistoryCommand> _validator;

        public UpdateCurrencyRateHistoryCommandHandler(ICurrencyRateHistoryRepository rateHistoryRepository,IUnitOfWork unitOfWork,IValidator<UpdateCurrencyRateHistoryCommand> validator)
        {
            _rateHistoryRepository = rateHistoryRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CurrencyRateHistoryDto> Handle(UpdateCurrencyRateHistoryCommand request,CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(
                    $"Validation failed: {string.Join(", ", errors)}");
            }

            var entity = await _rateHistoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                throw new Exception(
                    $"Currency rate history with id {request.Id} not found.");

            entity.Date = request.Date;
            entity.CloseRate = request.CloseRate;
            entity.OpenRate = request.OpenRate;
            entity.HighRate = request.HighRate;
            entity.LowRate = request.LowRate;

            _rateHistoryRepository.Update(entity);
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
