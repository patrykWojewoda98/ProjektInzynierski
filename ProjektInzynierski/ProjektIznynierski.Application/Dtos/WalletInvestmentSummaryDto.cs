namespace ProjektIznynierski.Application.Dtos;

public class WalletInvestmentSummaryDto
{
    public int InstrumentId { get; set; }
    public string InstrumentName { get; set; }

    public int TotalQuantity { get; set; }

    public string InstrumentCurrency { get; set; }
    public decimal PricePerUnit { get; set; }

    public decimal ValueInAccountCurrency { get; set; }
}