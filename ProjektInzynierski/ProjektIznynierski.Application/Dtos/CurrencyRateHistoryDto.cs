namespace ProjektIznynierski.Application.Dtos
{
    public class CurrencyRateHistoryDto
    {
        public int Id { get; set; }
        public int CurrencyPairId { get; set; }
        public DateTime Date { get; set; }
        public decimal CloseRate { get; set; }
    }
}
