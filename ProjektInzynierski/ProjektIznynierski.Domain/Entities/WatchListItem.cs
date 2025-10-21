namespace ProjektIznynierski.Domain.Entities
{
    public class WatchListItem : BaseEntity
    {
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public int WatchListId { get; set; }
        public WatchList WatchList { get; set; }

        public int InvestInstrumentId { get; set; }
        public InvestInstrument InvestInstrument { get; set; }
        
    }
}
