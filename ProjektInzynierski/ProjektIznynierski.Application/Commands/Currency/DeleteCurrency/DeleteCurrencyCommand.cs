using MediatR;

namespace ProjektIznynierski.Application.Commands.Currency.DeleteCurrency
{
    public class DeleteCurrencyCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
