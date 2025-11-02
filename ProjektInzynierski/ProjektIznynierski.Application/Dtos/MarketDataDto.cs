namespace ProjektIznynierski.Application.Dtos
{
    public class MarketDataDto
    {
        public int Id { get; set; }
        public int InvestInstrumentId { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public long Volume { get; set; }
        public decimal DailyChange { get; set; }
        public decimal DailyChangePercent { get; set; }
    }
}
