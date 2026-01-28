using MediatR;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.DeleteCurrencyPair
{
    public class DeleteCurrencyPairCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
