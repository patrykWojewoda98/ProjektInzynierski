using FluentValidation;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.InvestHorizon.UpdateInvestHorizon
{
    internal class UpdateInvestHorizonCommandHandler
        : IRequestHandler<UpdateInvestHorizonCommand, InvestHorizonDto>
    {
        private readonly IInvestHorizonRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateInvestHorizonCommand> _validator;

        public UpdateInvestHorizonCommandHandler(
            IInvestHorizonRepository repository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateInvestHorizonCommand> validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<InvestHorizonDto> Handle(
            UpdateInvestHorizonCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(
                    $"Validation failed. Reasons: {string.Join(", ", errors)}");
            }

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestHorizon with id {request.Id} not found.");
            }

            entity.Horizon = request.Horizon;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestHorizonDto
            {
                Id = entity.Id,
                Horizon = entity.Horizon
            };
        }
    }
}
