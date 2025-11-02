using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Sector.GetSectorById
{
    internal class GetSectorByIdQueryHandler : IRequestHandler<GetSectorByIdQuery, SectorDto>
    {
        private readonly ISectorRepository _repository;
        public GetSectorByIdQueryHandler(ISectorRepository repository)
        {
            _repository = repository;
        }

        public async Task<SectorDto> Handle(GetSectorByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Sector with id {request.id} not found.");
            }

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
