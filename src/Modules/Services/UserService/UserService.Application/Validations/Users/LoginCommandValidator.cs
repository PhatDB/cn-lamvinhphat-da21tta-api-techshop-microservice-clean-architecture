using FluentValidation;
using UserService.Application.Commands.Users.Login;

namespace UserService.Application.Validations.Users
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}