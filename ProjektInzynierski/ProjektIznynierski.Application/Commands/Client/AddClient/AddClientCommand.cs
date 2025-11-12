using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Client.AddClient
{
    public class AddClientCommand : IRequest<ClientDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public int? WalletId { get; set; }
        public int? InvestProfileId { get; set; }
    }
}
