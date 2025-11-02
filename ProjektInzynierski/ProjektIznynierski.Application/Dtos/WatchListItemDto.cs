namespace ProjektIznynierski.Application.Dtos
{
    public class WatchListItemDto
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public int WatchListId { get; set; }
        public int InvestInstrumentId { get; set; }
    }
}
