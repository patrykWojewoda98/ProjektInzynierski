using MediatR;
namespace ProjektIznynierski.Application.Commands.InvestHorizon.DeleteInvestHorizon
{
    public class DeleteInvestHorizonCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}