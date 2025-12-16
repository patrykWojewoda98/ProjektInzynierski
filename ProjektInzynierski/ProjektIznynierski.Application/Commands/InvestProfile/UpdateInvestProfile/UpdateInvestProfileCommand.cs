using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.InvestProfile.UpdateInvestProfile
{
    public class UpdateInvestProfileCommand : IRequest<InvestProfileDto>
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public int AcceptableRiskLevelId { get; set; }
        public int InvestHorizon { get; set; }
        public double? TargetReturn { get; set; }
        public double? MaxDrawDown { get; set; }
        public int? ClientId { get; set; }
    }
}
