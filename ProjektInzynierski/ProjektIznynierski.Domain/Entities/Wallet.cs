namespace ProjektIznynierski.Domain.Entities
{
    public class Wallet
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public decimal CashBalance { get; set; }     
        public int CurrencyId { get; set; }          
        public Currency Currency { get; set; }

        public ICollection<WalletInstrument> Instruments { get; set; }

    }
}

