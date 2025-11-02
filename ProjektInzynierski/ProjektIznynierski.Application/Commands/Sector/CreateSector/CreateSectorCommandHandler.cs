using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Sector.CreateSector
{
    internal class CreateSectorCommandHandler : IRequestHandler<CreateSectorCommand, SectorDto>
    {
        private readonly ISectorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateSectorCommandHandler(ISectorRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SectorDto> Handle(CreateSectorCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Sector
            {
                Name = request.Name,
                Code = request.Code,
                Description = request.Description
            };
            _repository.Add(entity);
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
