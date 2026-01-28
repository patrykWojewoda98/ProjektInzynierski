namespace ProjektIznynierski.Application.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int InvestInstrumentID { get; set; }
        public DateTime dateTime { get; set; }
    }
}
