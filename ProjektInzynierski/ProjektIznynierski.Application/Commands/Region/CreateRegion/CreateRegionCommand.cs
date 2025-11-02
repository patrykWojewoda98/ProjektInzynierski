using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Region.CreateRegion
{
    public class CreateRegionCommand : IRequest<RegionDto>
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public int RegionRisk { get; set; }
    }
}
