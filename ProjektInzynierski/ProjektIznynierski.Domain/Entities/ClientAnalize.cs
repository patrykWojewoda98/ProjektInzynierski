namespace ProjektIznynierski.Domain.Entities
{
    public class ClientAnalize
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int AnalyzeId { get; set; }
        public Analize Analyze { get; set; }
    }
}
