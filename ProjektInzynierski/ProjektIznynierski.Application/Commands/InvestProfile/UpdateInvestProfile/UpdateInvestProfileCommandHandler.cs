using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.InvestProfile.UpdateInvestProfile
{
    internal class UpdateInvestProfileCommandHandler : IRequestHandler<UpdateInvestProfileCommand, InvestProfileDto>
    {
        private readonly IInvestProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInvestProfileCommandHandler(IInvestProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InvestProfileDto> Handle(UpdateInvestProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestProfile with id {request.Id} not found.");
            }

            entity.ProfileName = request.ProfileName;
            entity.AcceptableRiskLevelId = request.AcceptableRiskLevelId;
            entity.InvestHorizonId = request.InvestHorizonId;
            entity.TargetReturn = request.TargetReturn;
            entity.MaxDrawDown = request.MaxDrawDown;
            entity.ClientId = request.ClientId;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestProfileDto
            {
                Id = entity.Id,
                ProfileName = entity.ProfileName,
                AcceptableRiskLevelId = entity.AcceptableRiskLevelId,
                InvestHorizonId = entity.InvestHorizonId,
                TargetReturn = entity.TargetReturn,
                MaxDrawDown = entity.MaxDrawDown,
                ClientId = entity.ClientId
            };
        }
    }
}
