using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Client.UpdateClient
{
    public class UpdateClientCommand : IRequest<ClientDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int CountryId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
