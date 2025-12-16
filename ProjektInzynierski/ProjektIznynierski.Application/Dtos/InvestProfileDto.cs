namespace ProjektIznynierski.Application.Dtos
{
    public class InvestProfileDto
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public int AcceptableRisk { get; set; }
        public int? InvestHorizonId { get; set; }
        public double? TargetReturn { get; set; }
        public double? MaxDrawDown { get; set; }
        public int? ClientId { get; set; }
    }
}
