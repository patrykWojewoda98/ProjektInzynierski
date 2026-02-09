using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Client.UpdateClient
{
    public class UpdateClientCommandValidation : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Client full name is required.")
                .MaximumLength(150).WithMessage("Client full name cannot exceed 150 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .MaximumLength(255).WithMessage("Email address cannot exceed 255 characters.");

            RuleFor(x => x.City)
                .MaximumLength(150)
                .WithMessage("City name cannot exceed 150 characters.");

            RuleFor(x => x.Address)
                .MaximumLength(255)
                .WithMessage("Address cannot exceed 255 characters.");

            RuleFor(x => x.PostalCode)
                .MaximumLength(20)
                .WithMessage("Postal code cannot exceed 20 characters.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0)
                .WithMessage("Country identifier is required and must be greater than zero.");

            RuleFor(x => x.NewPassword)
                .MinimumLength(6)
                .When(x => !string.IsNullOrEmpty(x.NewPassword))
                .WithMessage("New password must be at least 6 characters long.");
        }
    }
}
