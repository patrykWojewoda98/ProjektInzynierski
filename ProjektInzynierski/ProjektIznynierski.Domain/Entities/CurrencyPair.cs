namespace ProjektIznynierski.Domain.Entities
{
    public class CurrencyPair : BaseEntity
    {
        public int BaseCurrencyId { get; set; }
        public Currency BaseCurrency { get; set; }
        public int QuoteCurrencyId { get; set; }
        public Currency QuoteCurrency { get; set; }

        public string Symbol { get; set; }

        public ICollection<CurrencyRateHistory> RateHistory { get; set; }
            = new List<CurrencyRateHistory>();
    }
}
