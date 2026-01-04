using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RegionCode.DeleteRegionCode
{
    internal class DeleteRegionCodeCommandHandler: IRequestHandler<DeleteRegionCodeCommand, Unit>
    {
        private readonly IRegionCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRegionCodeCommandHandler(IRegionCodeRepository repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteRegionCodeCommand request,CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                throw new Exception($"RegionCode with id {request.Id} not found.");
            }

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
