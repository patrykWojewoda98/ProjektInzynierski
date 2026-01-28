namespace ProjektIznynierski.Domain.Entities
{
    public class CurrencyRateHistory : BaseEntity
    {
        public int CurrencyPairId { get; set; }
        public CurrencyPair CurrencyPair { get; set; }

        public DateTime Date { get; set; }
        public decimal CloseRate { get; set; }

        public decimal? OpenRate { get; set; }
        public decimal? HighRate { get; set; }
        public decimal? LowRate { get; set; }
    }
}
