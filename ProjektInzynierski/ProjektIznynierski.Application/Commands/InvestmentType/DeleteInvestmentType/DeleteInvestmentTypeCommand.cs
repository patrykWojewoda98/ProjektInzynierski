using MediatR;

namespace ProjektIznynierski.Application.Commands.InvestmentType.DeleteInvestmentType
{
    public class DeleteInvestmentTypeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}