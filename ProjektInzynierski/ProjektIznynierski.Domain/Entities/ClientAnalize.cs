namespace ProjektIznynierski.Domain.Entities
{
    public class ClientAnalize
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int AnalizeId { get; set; }
        public AIAnalysisResult AIAnalysisResult { get; set; }
    }
}
