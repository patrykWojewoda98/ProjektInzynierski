using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentById;

internal class GetWalletInstrumentByIdQueryHandler
    : IRequestHandler<GetWalletInstrumentByIdQuery, WalletInstrumentDto>
{
    private readonly IWalletInstrumentRepository _repository;

    public GetWalletInstrumentByIdQueryHandler(
        IWalletInstrumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<WalletInstrumentDto> Handle(
        GetWalletInstrumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException(
                $"WalletInstrument with id {request.Id} not found");

        return new WalletInstrumentDto
        {
            Id = entity.Id,
            WalletId = entity.WalletId,
            InvestInstrumentId = entity.InvestInstrumentId,
            Quantity = entity.Quantity,
            CreatedAt = entity.CreatedAt
        };
    }
}
