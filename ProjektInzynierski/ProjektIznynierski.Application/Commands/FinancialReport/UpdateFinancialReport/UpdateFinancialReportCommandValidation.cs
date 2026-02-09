using FluentValidation;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialReport.UpdateFinancialReport
{
    public class UpdateFinancialReportCommandValidation : AbstractValidator<UpdateFinancialReportCommand>
    {
        public UpdateFinancialReportCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Financial report identifier is required and must be greater than zero.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("Investment instrument identifier is required and must be greater than zero.");

            RuleFor(x => x.Period)
                .NotEmpty()
                .WithMessage("Reporting period is required.")
                .MaximumLength(50)
                .WithMessage("Reporting period cannot exceed 50 characters.");

            RuleFor(x => x.Revenue)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Revenue.HasValue)
                .WithMessage("Revenue cannot be negative.");

            RuleFor(x => x.NetIncome)
                .GreaterThanOrEqualTo(0)
                .When(x => x.NetIncome.HasValue)
                .WithMessage("Net income cannot be negative.");

            RuleFor(x => x.EPS)
                .GreaterThanOrEqualTo(0)
                .When(x => x.EPS.HasValue)
                .WithMessage("EPS cannot be negative.");

            RuleFor(x => x.Assets)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Assets.HasValue)
                .WithMessage("Assets cannot be negative.");

            RuleFor(x => x.Liabilities)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Liabilities.HasValue)
                .WithMessage("Liabilities cannot be negative.");
        }
    }
}
