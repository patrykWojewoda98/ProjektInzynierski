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
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Pesel)
                .NotEmpty()
                .Length(11);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Password)
                .MinimumLength(6);
        }
    }
}
