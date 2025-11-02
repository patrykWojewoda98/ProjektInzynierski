using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.Currency.CreateCurrency
{
    internal class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, CurrencyDto>
    {
        private readonly ICurrencyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateCurrencyCommandHandler(ICurrencyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CurrencyDto> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Currency
            {
                Name = request.Name,
                CurrencyRisk = (RiskLevel)request.CurrencyRisk
            };
            _repository.Add(entity);
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
