using MediatR;
using ProjektIznynierski.Application.Dtos;
namespace ProjektIznynierski.Application.Commands.InvestHorizon.AddInvestHorizon
{
    public class AddInvestHorizonCommand : IRequest<InvestHorizonDto>
    {
        public string Horizon { get; set; }
    }
}