namespace ProjektIznynierski.Application.Dtos
{
    public class WalletDto
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public decimal CashBalance { get; set; }
        public int CurrencyId { get; set; }
    }
}
