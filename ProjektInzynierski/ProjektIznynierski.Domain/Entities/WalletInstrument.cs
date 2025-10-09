namespace ProjektIznynierski.Domain.Entities
{
    public class WalletInstrument : BaseEntity
    {
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public int InvestInstrumentId { get; set; }
        public InvestInstrument InvestInstrument { get; set; }
        public decimal Quantity { get; set; }
    }
}
