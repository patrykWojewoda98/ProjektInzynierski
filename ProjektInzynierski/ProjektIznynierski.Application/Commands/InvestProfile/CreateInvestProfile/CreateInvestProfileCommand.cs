using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.InvestProfile.CreateInvestProfile
{
    public class CreateInvestProfileCommand : IRequest<InvestProfileDto>
    {
        public string ProfileName { get; set; }
        public int AcceptableRisk { get; set; }
        public int InvestHorizonID { get; set; }
        public double? TargetReturn { get; set; }
        public double? MaxDrawDown { get; set; }
        public int? ClientId { get; set; }
    }
}
