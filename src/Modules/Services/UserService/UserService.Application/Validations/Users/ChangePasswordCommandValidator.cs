using FluentValidation;
using UserService.Application.Commands.Users.ChangePassword;

namespace UserService.Application.Validations.Users
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Invalid user ID.");

            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required.").MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .NotEqual(x => x.OldPassword).WithMessage("New password cannot be the same as the old password.");
        }
    }
}