using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.ReorderClientInterfaceConfig
{
    internal class ReorderClientInterfaceConfigCommandHandler : IRequestHandler<ReorderClientInterfaceConfigCommand, Unit>
    {
        private readonly IClientInterfaceConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderClientInterfaceConfigCommandHandler(IClientInterfaceConfigRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReorderClientInterfaceConfigCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items)
            {
                var entity = await _repository.GetByIdAsync(item.Id, cancellationToken);
                if (entity != null)
                {
                    entity.OrderIndex = item.OrderIndex;
                    _repository.Update(entity);
                }
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
