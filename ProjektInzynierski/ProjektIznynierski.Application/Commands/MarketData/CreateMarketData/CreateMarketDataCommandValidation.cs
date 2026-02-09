using FluentValidation;

namespace ProjektIznynierski.Application.Commands.MarketData.CreateMarketData
{
    public class CreateMarketDataCommandValidation : AbstractValidator<CreateMarketDataCommand>
    {
        public CreateMarketDataCommandValidation()
        {
            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("Investment instrument identifier is required and must be greater than zero.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Quotation date is required.");

            RuleFor(x => x.OpenPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Open price cannot be negative.");

            RuleFor(x => x.ClosePrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Close price cannot be negative.");

            RuleFor(x => x.HighPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("High price cannot be negative.");

            RuleFor(x => x.LowPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Low price cannot be negative.");

            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Volume cannot be negative.");
        }
    }
}
