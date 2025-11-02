using MediatR;

namespace ProjektIznynierski.Application.Commands.Country.DeleteCountry
{
    public class DeleteCountryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
