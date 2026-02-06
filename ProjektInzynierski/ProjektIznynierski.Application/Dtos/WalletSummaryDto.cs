namespace ProjektIznynierski.Application.Dtos
{
    public class WalletSummaryDto
    {
        public string AccountCurrency { get; set; }

        public decimal CashBalance { get; set; }
        public decimal TotalInvestmentsValue { get; set; }

        public decimal TotalAccountValue =>
            CashBalance + TotalInvestmentsValue;

        public List<WalletInvestmentSummaryDto> Investments { get; set; }
    }
}
