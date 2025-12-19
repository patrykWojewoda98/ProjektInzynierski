using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RiskLevel.UpdateRiskLevel
{
    internal class UpdateRiskLevelCommandHandler
        : IRequestHandler<UpdateRiskLevelCommand, RiskLevelDto>
    {
        private readonly IRiskLevelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRiskLevelCommandHandler(
            IRiskLevelRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RiskLevelDto> Handle(
            UpdateRiskLevelCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"RiskLevel with id {request.Id} not found.");
            }

            entity.RiskLevelScale = request.RiskScale;
            entity.Description = request.Description;

            _repository.Update(entity);
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
