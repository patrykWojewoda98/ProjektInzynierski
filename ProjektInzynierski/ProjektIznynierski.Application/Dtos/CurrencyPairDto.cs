namespace ProjektIznynierski.Application.Dtos
{
    public class CurrencyPairDto
    {
        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int QuoteCurrencyId { get; set; }
        public string Symbol { get; set; }
    }
}
