namespace ProjektIznynierski.Domain.Entities
{
    public class Analize : BaseEntity
    {

        public string Description { get; set; }

        public ICollection<ClientAnalize> ClientAnalyzes { get; set; }
    }
}
