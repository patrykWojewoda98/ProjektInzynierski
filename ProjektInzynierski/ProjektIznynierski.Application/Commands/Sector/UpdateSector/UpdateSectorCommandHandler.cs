using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Sector.UpdateSector
{
    internal class UpdateSectorCommandHandler : IRequestHandler<UpdateSectorCommand, SectorDto>
    {
        private readonly ISectorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateSectorCommandHandler(ISectorRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SectorDto> Handle(UpdateSectorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Sector with id {request.Id} not found.");
            }

            entity.Name = request.Name;
            entity.Code = request.Code;
            entity.Description = request.Description;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new SectorDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description
            };
        }
    }
}
