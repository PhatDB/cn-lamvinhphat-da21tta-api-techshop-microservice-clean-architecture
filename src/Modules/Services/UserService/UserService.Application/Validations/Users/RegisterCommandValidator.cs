using FluentValidation;
using UserService.Application.Commands.Users.Register;

namespace UserService.Application.Validations.Users
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.").MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}