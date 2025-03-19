using FluentValidation;
using UserService.Application.Commands.Users.Update;

namespace UserService.Application.Validations.Users
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty.").MaximumLength(255).WithMessage("Username must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Username));

            RuleFor(x => x.PhoneNumber).Matches(@"^\d{10,15}$").WithMessage("Phone number must be between 10-15 digits.").When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.AddressLine).MaximumLength(255).WithMessage("Address must not exceed 255 characters.").When(x => !string.IsNullOrEmpty(x.AddressLine));

            RuleFor(x => x.Province).MaximumLength(50).WithMessage("Province must not exceed 50 characters.").When(x => !string.IsNullOrEmpty(x.Province));

            RuleFor(x => x.District).MaximumLength(50).WithMessage("District must not exceed 50 characters.").When(x => !string.IsNullOrEmpty(x.District));
        }
    }
}