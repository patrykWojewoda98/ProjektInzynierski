using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.UpdateFinancialMetric
{
    internal class UpdateFinancialMetricCommandHandler : IRequestHandler<UpdateFinancialMetricCommand, FinancialMetricDto>
    {
        private readonly IFinancialMetricRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateFinancialMetricCommandHandler(IFinancialMetricRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FinancialMetricDto> Handle(UpdateFinancialMetricCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialMetric with id {request.Id} not found.");
            }

            entity.PE = request.PE;
            entity.PB = request.PB;
            entity.ROE = request.ROE;
            entity.DebtToEquity = request.DebtToEquity;
            entity.DividendYield = request.DividendYield;

            _repository.Update(entity);
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
