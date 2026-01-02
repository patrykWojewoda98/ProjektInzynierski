using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Employee.UpdateEmployee
{
    public class UpdateEmployeeCommandValidation : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.NewPassword)
                .MinimumLength(6)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword))
                .WithMessage("New password must be at least 6 characters.");

            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword))
                .WithMessage("Old password is required when changing password.");
        }
    }
}
