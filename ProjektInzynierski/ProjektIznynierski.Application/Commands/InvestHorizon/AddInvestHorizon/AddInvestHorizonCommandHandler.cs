using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.InvestHorizon.AddInvestHorizon
{
    internal class AddInvestHorizonCommandHandler : IRequestHandler<AddInvestHorizonCommand, InvestHorizonDto>
    {
        private readonly IInvestHorizonRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddInvestHorizonCommand> _validator;

        public AddInvestHorizonCommandHandler(IInvestHorizonRepository repository, IUnitOfWork unitOfWork, IValidator<AddInvestHorizonCommand> validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<InvestHorizonDto> Handle(AddInvestHorizonCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException(
                    $"Validation failed. Reasons: {string.Join(", ", errors)}");
            }

            var entity = new ProjektIznynierski.Domain.Entities.InvestHorizon
            {
                Horizon = request.Horizon
            };

            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestHorizonDto
            {
                Id = entity.Id,
                Horizon = entity.Horizon
            };
        }
    }
}
