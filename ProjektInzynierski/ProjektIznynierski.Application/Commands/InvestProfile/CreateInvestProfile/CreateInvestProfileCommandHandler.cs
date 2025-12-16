using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.InvestProfile.CreateInvestProfile
{
    internal class CreateInvestProfileCommandHandler : IRequestHandler<CreateInvestProfileCommand, InvestProfileDto>
    {
        private readonly IInvestProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateInvestProfileCommandHandler(IInvestProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InvestProfileDto> Handle(CreateInvestProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.InvestProfile
            {
                ProfileName = request.ProfileName,
                AcceptableRisk = (RiskLevel)request.AcceptableRisk,
                InvestHorizonId = request.InvestHorizonID,
                TargetReturn = request.TargetReturn,
                MaxDrawDown = request.MaxDrawDown,
                ClientId = request.ClientId
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestProfileDto
            {
                Id = entity.Id,
                ProfileName = entity.ProfileName,
                AcceptableRisk = (int)entity.AcceptableRisk,
                InvestHorizonId = entity.InvestHorizonId,
                TargetReturn = entity.TargetReturn,
                MaxDrawDown = entity.MaxDrawDown,
                ClientId = entity.ClientId
            };
        }
    }
}
