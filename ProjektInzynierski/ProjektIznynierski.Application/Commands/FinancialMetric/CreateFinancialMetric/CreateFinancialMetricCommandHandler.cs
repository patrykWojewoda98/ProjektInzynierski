using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric
{
    internal class CreateFinancialMetricCommandHandler : IRequestHandler<CreateFinancialMetricCommand, FinancialMetricDto>
    {
        private readonly IFinancialMetricRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateFinancialMetricCommandHandler(IFinancialMetricRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FinancialMetricDto> Handle(CreateFinancialMetricCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.FinancialMetric
            {
                PE = request.PE,
                PB = request.PB,
                ROE = request.ROE,
                DebtToEquity = request.DebtToEquity,
                DividendYield = request.DividendYield
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new FinancialMetricDto
            {
                Id = entity.Id,
                PE = entity.PE,
                PB = entity.PB,
                ROE = entity.ROE,
                DebtToEquity = entity.DebtToEquity,
                DividendYield = entity.DividendYield
            };
        }
    }
}
