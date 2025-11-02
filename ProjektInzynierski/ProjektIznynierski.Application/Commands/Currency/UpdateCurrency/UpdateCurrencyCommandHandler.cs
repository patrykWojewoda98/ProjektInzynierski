using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.Currency.UpdateCurrency
{
    internal class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
    {
        private readonly ICurrencyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCurrencyCommandHandler(ICurrencyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CurrencyDto> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Currency with id {request.Id} not found.");
            }

            entity.Name = request.Name;
            entity.CurrencyRisk = (RiskLevel)request.CurrencyRisk;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CurrencyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CurrencyRisk = (int)entity.CurrencyRisk
            };
        }
    }
}
