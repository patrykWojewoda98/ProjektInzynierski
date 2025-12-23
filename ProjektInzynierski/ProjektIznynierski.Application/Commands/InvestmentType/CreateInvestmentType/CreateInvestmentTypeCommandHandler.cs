using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    internal class CreateInvestmentTypeCommandHandler : IRequestHandler<CreateInvestmentTypeCommand, InvestmentTypeDto>
    {
        private readonly IInvestmentTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateInvestmentTypeCommandHandler(IInvestmentTypeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InvestmentTypeDto> Handle(CreateInvestmentTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.InvestmentType
            {
                TypeName = request.TypeName
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestmentTypeDto
            {
                Id = entity.Id,
                TypeName = entity.TypeName
            };
        }
    }
}
