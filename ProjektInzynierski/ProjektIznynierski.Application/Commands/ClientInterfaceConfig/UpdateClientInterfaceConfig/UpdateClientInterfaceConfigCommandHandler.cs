using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.UpdateClientInterfaceConfig
{
    internal class UpdateClientInterfaceConfigCommandHandler : IRequestHandler<UpdateClientInterfaceConfigCommand, ClientInterfaceConfigDto>
    {
        private readonly IClientInterfaceConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClientInterfaceConfigCommandHandler(IClientInterfaceConfigRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientInterfaceConfigDto> Handle(UpdateClientInterfaceConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new Exception($"ClientInterfaceConfig with id {request.Id} not found.");

            entity.Key = request.Key;
            entity.DisplayText = request.DisplayText;
            entity.Description = request.Description;
            entity.ImagePath = request.ImagePath;
            entity.OrderIndex = request.OrderIndex;
            entity.IsVisible = request.IsVisible;
            entity.ModifiedByEmployeeId = request.ModifiedByEmployeeId;

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
