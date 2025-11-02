namespace ProjektIznynierski.Application.Dtos
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int RegionId { get; set; }
        public int CurrencyId { get; set; }
        public int CountryRisk { get; set; }
    }
}
