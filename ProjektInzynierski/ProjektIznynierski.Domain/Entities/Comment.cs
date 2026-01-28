namespace ProjektIznynierski.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public int ClientID { get; set; }
        public int InvestInstrumentID { get; set; }
        public string Content { get; set; }
        public Client Client { get; set; }
        public InvestInstrument InvestInstrument { get; set; }
    }
}
