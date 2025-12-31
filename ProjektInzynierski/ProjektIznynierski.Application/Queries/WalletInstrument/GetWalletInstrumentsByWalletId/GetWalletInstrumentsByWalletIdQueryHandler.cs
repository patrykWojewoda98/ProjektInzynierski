using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentById;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentsByWalletId;

internal class GetWalletInstrumentsByWalletIdQueryHandler
    : IRequestHandler<GetWalletInstrumentsByWalletIdQuery, List<WalletInstrumentDto>>
{
    private readonly IWalletInstrumentRepository _repository;

    public GetWalletInstrumentsByWalletIdQueryHandler(
        IWalletInstrumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WalletInstrumentDto>> Handle(
        GetWalletInstrumentsByWalletIdQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository
            .GetByWalletIdAsync(request.WalletId, cancellationToken);

        return entities.Select(entity => new WalletInstrumentDto
        {
            Id = entity.Id,
            WalletId = entity.WalletId,
            InvestInstrumentId = entity.InvestInstrumentId,
            Quantity = entity.Quantity,
            CreatedAt = entity.CreatedAt
        }).ToList();
    }
}
