using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;


namespace ProjektIznynierski.Application.Commands.RegionCode.CreateRegionCode
{
    internal class CreateRegionCodeCommandHandler: IRequestHandler<CreateRegionCodeCommand, RegionCodeDto>
    {
        private readonly IRegionCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRegionCodeCommandHandler(IRegionCodeRepository repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegionCodeDto> Handle(CreateRegionCodeCommand request,CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.RegionCode
            {
                Code = request.Code
            };

            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new RegionCodeDto
            {
                Id = entity.Id,
                Code = entity.Code
            };
        }
    }
}
