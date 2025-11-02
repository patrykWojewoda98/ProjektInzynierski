using FluentValidation;

namespace ProjektIznynierski.Application.Commands.FinancialReport.UpdateFinancialReport
{
    public class UpdateFinancialReportCommandValidation : AbstractValidator<UpdateFinancialReportCommand>
    {
        public UpdateFinancialReportCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator raportu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.ReportDate)
                .NotEmpty().WithMessage("Data raportu jest wymagana.");

            RuleFor(x => x.Period)
                .NotEmpty().WithMessage("Okres raportu jest wymagany.")
                .MaximumLength(50).WithMessage("Okres raportu nie może przekraczać 50 znaków.");

            RuleFor(x => x.Revenue).GreaterThanOrEqualTo(0).When(x => x.Revenue.HasValue).WithMessage("Przychody nie mogą być ujemne.");
            RuleFor(x => x.NetIncome).GreaterThanOrEqualTo(0).When(x => x.NetIncome.HasValue).WithMessage("Zysk netto nie może być ujemny.");
            RuleFor(x => x.EPS).GreaterThanOrEqualTo(0).When(x => x.EPS.HasValue).WithMessage("EPS nie może być ujemny.");
            RuleFor(x => x.Assets).GreaterThanOrEqualTo(0).When(x => x.Assets.HasValue).WithMessage("Aktywa nie mogą być ujemne.");
            RuleFor(x => x.Liabilities).GreaterThanOrEqualTo(0).When(x => x.Liabilities.HasValue).WithMessage("Zobowiązania nie mogą być ujemne.");
            RuleFor(x => x.OperatingCashFlow).GreaterThanOrEqualTo(0).When(x => x.OperatingCashFlow.HasValue).WithMessage("Przepływy operacyjne nie mogą być ujemne.");
            RuleFor(x => x.FreeCashFlow).GreaterThanOrEqualTo(0).When(x => x.FreeCashFlow.HasValue).WithMessage("Wolne przepływy pieniężne nie mogą być ujemne.");
        }
    }
}
