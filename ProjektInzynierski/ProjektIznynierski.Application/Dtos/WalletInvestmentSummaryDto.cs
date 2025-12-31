namespace ProjektIznynierski.Application.Dtos;

public class WalletInvestmentSummaryDto
{
    public int InstrumentId { get; set; }
    public string InstrumentName { get; set; } = default!;
    public decimal TotalQuantity { get; set; }
}
