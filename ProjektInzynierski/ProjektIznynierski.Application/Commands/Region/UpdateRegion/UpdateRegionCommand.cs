using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Region.UpdateRegion
{
    public class UpdateRegionCommand : IRequest<RegionDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionCodeId { get; set; }
        public int RegionRiskLevelId { get; set; }
    }
}
