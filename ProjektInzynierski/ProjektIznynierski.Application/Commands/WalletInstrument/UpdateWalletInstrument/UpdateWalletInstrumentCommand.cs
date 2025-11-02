using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.UpdateWalletInstrument
{
    public class UpdateWalletInstrumentCommand : IRequest<WalletInstrumentDto>
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public int InvestInstrumentId { get; set; }
        public decimal Quantity { get; set; }
    }
}
