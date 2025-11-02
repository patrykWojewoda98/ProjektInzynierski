using MediatR;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.DeleteWalletInstrument
{
    public class DeleteWalletInstrumentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
