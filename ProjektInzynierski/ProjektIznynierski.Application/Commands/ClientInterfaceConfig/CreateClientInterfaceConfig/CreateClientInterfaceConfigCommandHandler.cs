using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.CreateClientInterfaceConfig
{
    internal class CreateClientInterfaceConfigCommandHandler : IRequestHandler<CreateClientInterfaceConfigCommand, ClientInterfaceConfigDto>
    {
        private readonly IClientInterfaceConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientInterfaceConfigCommandHandler(IClientInterfaceConfigRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientInterfaceConfigDto> Handle(CreateClientInterfaceConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.ClientInterfaceConfig
            {
                Platform = request.Platform,
                InterfaceType = ClientInterfaceType.Client,
                Key = request.Key,
                DisplayText = request.DisplayText,
                Description = request.Description,
                ImagePath = request.ImagePath,
                OrderIndex = request.OrderIndex,
                IsVisible = request.IsVisible,
                ModifiedByEmployeeId = request.ModifiedByEmployeeId
            };
            _repository.Add(entity);
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
