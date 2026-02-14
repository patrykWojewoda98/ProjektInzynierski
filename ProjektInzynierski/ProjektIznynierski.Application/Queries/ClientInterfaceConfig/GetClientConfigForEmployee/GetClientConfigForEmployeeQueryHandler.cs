using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Queries.ClientInterfaceConfig.GetClientConfigForEmployee
{
    internal class GetClientConfigForEmployeeQueryHandler : IRequestHandler<GetClientConfigForEmployeeQuery, List<ClientInterfaceConfigDto>>
    {
        private readonly IClientInterfaceConfigRepository _repository;

        public GetClientConfigForEmployeeQueryHandler(IClientInterfaceConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClientInterfaceConfigDto>> Handle(GetClientConfigForEmployeeQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetByPlatformAndInterfaceTypeAsync(
                request.Platform,
                ClientInterfaceType.Client,
                visibleOnly: false,
                cancellationToken);

            return items.Select(c => new ClientInterfaceConfigDto
            {
                Id = c.Id,
                Platform = c.Platform,
                InterfaceType = c.InterfaceType,
                Key = c.Key,
                DisplayText = c.DisplayText,
                Description = c.Description,
                ImagePath = c.ImagePath,
                OrderIndex = c.OrderIndex,
                IsVisible = c.IsVisible,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                ModifiedByEmployeeId = c.ModifiedByEmployeeId
            }).ToList();
        }
    }
}
