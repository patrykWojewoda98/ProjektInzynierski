namespace ProjektIznynierski.Domain.Entities
{
    public class ClientAnalyze
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int AnalyzeId { get; set; }
        public Analyze Analyze { get; set; }
    }
}
