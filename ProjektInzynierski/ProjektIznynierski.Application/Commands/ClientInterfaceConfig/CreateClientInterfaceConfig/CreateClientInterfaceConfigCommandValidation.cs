using FluentValidation;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.CreateClientInterfaceConfig
{
    public class CreateClientInterfaceConfigCommandValidation : AbstractValidator<CreateClientInterfaceConfigCommand>
    {
        public CreateClientInterfaceConfigCommandValidation()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key is required.")
                .MaximumLength(100).WithMessage("Key cannot exceed 100 characters.");
            RuleFor(x => x.DisplayText)
                .NotEmpty().WithMessage("DisplayText is required.")
                .MaximumLength(200).WithMessage("DisplayText cannot exceed 200 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.ImagePath)
                .MaximumLength(500).WithMessage("ImagePath cannot exceed 500 characters.");
        }
    }
}
