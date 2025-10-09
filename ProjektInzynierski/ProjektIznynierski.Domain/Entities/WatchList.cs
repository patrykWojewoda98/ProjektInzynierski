namespace ProjektIznynierski.Domain.Entities
{
    public class WatchList : BaseEntity
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<WatchListItem> Items { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
