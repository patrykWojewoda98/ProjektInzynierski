using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RegionCode.UpdateRegionCode
{
    internal class UpdateRegionCodeCommandHandler: IRequestHandler<UpdateRegionCodeCommand, RegionCodeDto>
    {
        private readonly IRegionCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRegionCodeCommandHandler(IRegionCodeRepository repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegionCodeDto> Handle(UpdateRegionCodeCommand request,CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                throw new Exception($"RegionCode with id {request.Id} not found.");
            }

            entity.Code = request.Code;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return new RegionCodeDto
            {
                Id = entity.Id,
                Code = entity.Code
            };
        }
    }
}
