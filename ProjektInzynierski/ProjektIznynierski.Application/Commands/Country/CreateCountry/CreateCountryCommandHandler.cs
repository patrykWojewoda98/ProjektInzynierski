using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    internal class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, CountryDto>
    {
        private readonly ICountryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateCountryCommandHandler(ICountryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CountryDto> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Country
            {
                Name = request.Name,
                IsoCode = request.IsoCode,
                RegionId = request.RegionId,
                CurrencyId = request.CurrencyId,
                CountryRiskLevelId = request.CountryRiskLevelId
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CountryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsoCode = entity.IsoCode,
                RegionId = entity.RegionId,
                CurrencyId = entity.CurrencyId,
                CountryRiskLevelId = (int)entity.CountryRiskLevelId
            };
        }
    }
}
