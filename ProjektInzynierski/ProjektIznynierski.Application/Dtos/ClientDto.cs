using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Application.Dtos
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int? InvestProfileId { get; set; }
        public int? WalletId { get; set; }
    }
}
