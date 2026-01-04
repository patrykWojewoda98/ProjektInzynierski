using MediatR;

namespace ProjektIznynierski.Application.Commands.RegionCode.DeleteRegionCode
{
    public class DeleteRegionCodeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}