using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.RegionCode.CreateRegionCode
{
    public class CreateRegionCodeCommand : IRequest<RegionCodeDto>
    {
        public string Code { get; set; }
    }
}
