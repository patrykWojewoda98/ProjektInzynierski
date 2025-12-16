using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.Region.CreateRegion
{
    internal class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, RegionDto>
    {
        private readonly IRegionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateRegionCommandHandler(IRegionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegionDto> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Region
            {
                Name = request.Name,
                RegionCodeId = request.RegionCodeId,
                RegionRiskLevelId = request.RegionRiskLevelId   
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new RegionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                RegionCodeId = entity.RegionCodeId,
                RegionRiskLevelId = entity.RegionRiskLevelId
            };
        }
    }
}
