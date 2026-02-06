namespace ProjektInzynierski.Domain.Models
{
    public class WalletSnapshot
    {
        public int WalletId { get; set; }

        public string AccountCurrency { get; set; } = string.Empty;

        public decimal CashAmount { get; set; }

        public decimal TotalValueInAccountCurrency { get; set; }

        public List<WalletInstrumentSnapshot> Instruments { get; set; }
            = new();
    }
}
