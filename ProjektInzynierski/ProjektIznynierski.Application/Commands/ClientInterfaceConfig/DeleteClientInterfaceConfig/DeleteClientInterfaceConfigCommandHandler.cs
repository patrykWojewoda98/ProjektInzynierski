using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.DeleteClientInterfaceConfig
{
    internal class DeleteClientInterfaceConfigCommandHandler : IRequestHandler<DeleteClientInterfaceConfigCommand, Unit>
    {
        private readonly IClientInterfaceConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClientInterfaceConfigCommandHandler(IClientInterfaceConfigRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteClientInterfaceConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new Exception($"ClientInterfaceConfig with id {request.Id} not found.");
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
