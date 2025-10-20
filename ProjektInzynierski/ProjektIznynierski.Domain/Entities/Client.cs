namespace ProjektIznynierski.Domain.Entities
{
    public class Client : BaseEntity
    {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Wallet { get; set; }

            public int InvestProfileId { get; set; }
            public InvestProfile InvestProfile { get; set; }

            public string City { get; set; }
            public string Address { get; set; }
            public string PostalCode { get; set; }

            public int CountryId { get; set; }
            public Country Country { get; set; }

            public ICollection<AIAnalysisResult> AIAnalysisResults { get; set; } = new List<AIAnalysisResult>();
            public ICollection<WatchList> WatchLists { get; set; }
    }
    
}