using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.RegionCode.UpdateRegionCode
{
    public class UpdateRegionCodeCommand : IRequest<RegionCodeDto>
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
