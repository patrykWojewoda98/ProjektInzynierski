namespace ProjektIznynierski.Domain.Entities
{
    public class MarketData : BaseEntity
    {
        public int InvestInstrumentId { get; set; }
        public InvestInstrument InvestInstrument { get; set; }

        public DateTime Date { get; set; }

        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }

        public long Volume { get; set; }

        public decimal DailyChange => ClosePrice - OpenPrice;

        public decimal DailyChangePercent =>
            OpenPrice != 0 ? Math.Round((ClosePrice - OpenPrice) / OpenPrice * 100, 2) : 0;
    }
}
