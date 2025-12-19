using MediatR;

namespace ProjektIznynierski.Application.Commands.RiskLevel.DeleteRiskLevel
{
    public class DeleteRiskLevelCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
