namespace ProjektIznynierski.Application.Dtos
{
    public class WalletInstrumentDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public int InvestInstrumentId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
