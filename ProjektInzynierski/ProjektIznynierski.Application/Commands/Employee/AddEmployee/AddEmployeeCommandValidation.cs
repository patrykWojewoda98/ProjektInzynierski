using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Employee.AddEmployee
{
    public class AddEmployeeCommandValidation
        : AbstractValidator<AddEmployeeCommand>
    {
        public AddEmployeeCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(150)
                .WithMessage("Name cannot be longer than 150 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Pesel)
                .NotEmpty()
                .WithMessage("PESEL is required.")
                .Length(11)
                .WithMessage("PESEL must contain exactly 11 digits.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                .MaximumLength(20)
                .WithMessage("Phone number cannot be longer than 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
