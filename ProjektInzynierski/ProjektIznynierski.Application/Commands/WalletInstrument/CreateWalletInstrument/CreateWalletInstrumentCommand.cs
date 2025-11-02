using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.CreateWalletInstrument
{
    public class CreateWalletInstrumentCommand : IRequest<WalletInstrumentDto>
    {
        public int WalletId { get; set; }
        public int InvestInstrumentId { get; set; }
        public decimal Quantity { get; set; }
    }
}
