namespace ProjektInzynierski.Domain.Models
{
    public class WalletInstrumentSnapshot
    {
        public int WalletInstrumentId { get; set; }

        public string InstrumentName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string InstrumentCurrency { get; set; } = string.Empty;

        public decimal PositionValue { get; set; }

        public decimal PositionValueInAccountCurrency { get; set; }
    }
}
