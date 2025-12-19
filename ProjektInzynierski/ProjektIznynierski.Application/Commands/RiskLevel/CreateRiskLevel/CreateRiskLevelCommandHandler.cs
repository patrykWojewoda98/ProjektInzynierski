using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    internal class CreateRiskLevelCommandHandler : IRequestHandler<CreateRiskLevelCommand, RiskLevelDto>
    {
        private readonly IRiskLevelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateRiskLevelCommandHandler(IRiskLevelRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RiskLevelDto> Handle(CreateRiskLevelCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.RiskLevel
            {
                RiskLevelScale = request.RiskScale,
                Description = request.Description
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new RiskLevelDto
            {
                Id = entity.Id,
                RiskScale = entity.RiskLevelScale,
                Description = entity.Description
            };
        }
    }
}
