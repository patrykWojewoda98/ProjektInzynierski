using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Entities
{
    public class TradeHistory : BaseEntity
    {
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public int InvestInstrumentId { get; set; }
        public InvestInstrument InvestInstrument { get; set; }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public TradeType Type { get; set; } 
        public DateTime TradeDate { get; set; } = DateTime.UtcNow;
    }
}
