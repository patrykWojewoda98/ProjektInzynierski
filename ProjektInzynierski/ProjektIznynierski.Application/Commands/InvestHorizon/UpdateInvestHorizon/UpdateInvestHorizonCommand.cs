using MediatR;
using ProjektIznynierski.Application.Dtos;
namespace ProjektIznynierski.Application.Commands.InvestHorizon.UpdateInvestHorizon
{
    public class UpdateInvestHorizonCommand : IRequest<InvestHorizonDto>
    {
        public int Id { get; set; }
        public string Horizon { get; set; }
    }
}