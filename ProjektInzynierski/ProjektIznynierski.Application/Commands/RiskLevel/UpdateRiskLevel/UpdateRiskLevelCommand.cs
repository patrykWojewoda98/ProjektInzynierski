using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.RiskLevel.UpdateRiskLevel
{
    public class UpdateRiskLevelCommand : IRequest<RiskLevelDto>
    {
        public int Id { get; set; }
        public int RiskScale { get; set; }
        public string Description { get; set; }
    }
}
