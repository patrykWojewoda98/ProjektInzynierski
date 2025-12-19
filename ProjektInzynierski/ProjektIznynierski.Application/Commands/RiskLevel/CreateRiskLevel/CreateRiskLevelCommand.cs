using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    public class CreateRiskLevelCommand : IRequest<RiskLevelDto>
    {
        public int RiskScale { get; set; }
        public string Description { get; set; }
    }
}
