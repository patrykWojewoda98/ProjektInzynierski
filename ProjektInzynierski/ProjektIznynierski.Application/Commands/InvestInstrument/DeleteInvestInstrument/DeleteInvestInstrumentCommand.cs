using MediatR;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.DeleteInvestInstrument
{
    public class DeleteInvestInstrumentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
