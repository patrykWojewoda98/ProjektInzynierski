using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Queries.ClientInterfaceConfig.GetClientMenuConfig
{
    internal class GetClientMenuConfigQueryHandler : IRequestHandler<GetClientMenuConfigQuery, List<ClientMenuConfigItemDto>>
    {
        private readonly IClientInterfaceConfigRepository _repository;

        public GetClientMenuConfigQueryHandler(IClientInterfaceConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClientMenuConfigItemDto>> Handle(GetClientMenuConfigQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetByPlatformAndInterfaceTypeAsync(
                request.Platform,
                ClientInterfaceType.Client,
                visibleOnly: true,
                cancellationToken);

            return items.Select(c => new ClientMenuConfigItemDto
            {
                Id = c.Id,
                Key = c.Key,
                DisplayText = c.DisplayText,
                Description = c.Description,
                ImagePath = c.ImagePath,
                OrderIndex = c.OrderIndex
            }).ToList();
        }
    }
}
