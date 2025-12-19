namespace ProjektIznynierski.Application.Dtos
{
    public class TradeHistoryDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public int InvestInstrumentId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int TradeTypeId { get; set; }
        public DateTime TradeDate { get; set; }
    }
}
