using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Client.UpdateClient
{
    public class UpdateClientCommandValidation : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidation()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Imię i nazwisko klienta jest wymagane.")
                .MaximumLength(150).WithMessage("Imię i nazwisko nie może przekraczać 150 znaków.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
                .EmailAddress().WithMessage("Podano nieprawidłowy format adresu e-mail.")
                .MaximumLength(255).WithMessage("Adres e-mail nie może przekraczać 255 znaków.");

            RuleFor(x => x.City)
                .MaximumLength(150)
                .WithMessage("Nazwa miasta nie może przekraczać 150 znaków.");

            RuleFor(x => x.Address)
                .MaximumLength(255)
                .WithMessage("Adres nie może przekraczać 255 znaków.");

            RuleFor(x => x.PostalCode)
                .MaximumLength(20)
                .WithMessage("Kod pocztowy nie może przekraczać 20 znaków.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Identyfikator kraju jest wymagany i musi być większy od zera.");

            RuleFor(x => x.NewPassword)
                .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.NewPassword))
                .WithMessage("Nowe hasło musi mieć co najmniej 6 znaków.");
        }
    }
}
