using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.ToggleVisibilityClientInterfaceConfig
{
    internal class ToggleVisibilityClientInterfaceConfigCommandHandler : IRequestHandler<ToggleVisibilityClientInterfaceConfigCommand, ClientInterfaceConfigDto>
    {
        private readonly IClientInterfaceConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ToggleVisibilityClientInterfaceConfigCommandHandler(IClientInterfaceConfigRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientInterfaceConfigDto> Handle(ToggleVisibilityClientInterfaceConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new Exception($"ClientInterfaceConfig with id {request.Id} not found.");

            entity.IsVisible = !entity.IsVisible;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ClientInterfaceConfigDto
            {
                Id = entity.Id,
                Platform = entity.Platform,
                InterfaceType = entity.InterfaceType,
                Key = entity.Key,
                DisplayText = entity.DisplayText,
                Description = entity.Description,
                ImagePath = entity.ImagePath,
                OrderIndex = entity.OrderIndex,
                IsVisible = entity.IsVisible,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ModifiedByEmployeeId = entity.ModifiedByEmployeeId
            };
        }
    }
}
