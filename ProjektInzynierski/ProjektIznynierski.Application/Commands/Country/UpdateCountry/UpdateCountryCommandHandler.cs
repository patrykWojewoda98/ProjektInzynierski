using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    internal class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDto>
    {
        private readonly ICountryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCountryCommandHandler(ICountryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CountryDto> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Country with id {request.Id} not found.");
            }

            entity.Name = request.Name;
            entity.IsoCode = request.IsoCode;
            entity.RegionId = request.RegionId;
            entity.CurrencyId = request.CurrencyId;
            entity.CountryRisk = (RiskLevel)request.CountryRisk;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CountryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsoCode = entity.IsoCode,
                RegionId = entity.RegionId,
                CurrencyId = entity.CurrencyId,
                CountryRisk = (int)entity.CountryRisk
            };
        }
    }
}
