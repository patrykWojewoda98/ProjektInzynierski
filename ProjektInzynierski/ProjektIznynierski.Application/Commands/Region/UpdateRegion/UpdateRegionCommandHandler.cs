using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.Region.UpdateRegion
{
    internal class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, RegionDto>
    {
        private readonly IRegionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRegionCommandHandler(IRegionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegionDto> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Region with id {request.Id} not found.");
            }

            entity.Name = request.Name;
            entity.Code = (RegionCode)request.Code;
            entity.RegionRisk = (RiskLevel)request.RegionRisk;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new RegionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = (int)entity.Code,
                RegionRisk = (int)entity.RegionRisk
            };
        }
    }
}
