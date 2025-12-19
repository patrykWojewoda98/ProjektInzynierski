namespace ProjektIznynierski.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; }

        public int RegionCodeId { get; set; }

        public int RegionRiskLevelId { get; set; }

        public ICollection<Country> Countries { get; set; }
    }
}
