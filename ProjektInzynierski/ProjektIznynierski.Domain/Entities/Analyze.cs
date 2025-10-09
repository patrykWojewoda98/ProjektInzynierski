namespace ProjektIznynierski.Domain.Entities
{
    public class Analyze : BaseEntity
    {

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ClientAnalyze> ClientAnalyzes { get; set; }
    }
}
