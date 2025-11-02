using MediatR;

namespace ProjektIznynierski.Application.Commands.InvestProfile.DeleteInvestProfile
{
    public class DeleteInvestProfileCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
