using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    internal class UpdateInvestmentTypeCommandHandler : IRequestHandler<UpdateInvestmentTypeCommand, InvestmentTypeDto>
    {
        private readonly IInvestmentTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInvestmentTypeCommandHandler(IInvestmentTypeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InvestmentTypeDto> Handle(UpdateInvestmentTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestmentType with id {request.Id} not found.");
            }

            entity.TypeName = request.TypeName;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestmentTypeDto
            {
                Id = entity.Id,
                TypeName = entity.TypeName,
            };
        }
    }
}
