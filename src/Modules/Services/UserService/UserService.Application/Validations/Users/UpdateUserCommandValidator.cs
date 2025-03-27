using FluentValidation;
using UserService.Application.Commands.Users.Update;

namespace UserService.Application.Validations.Users
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be a positive integer.");

            RuleFor(x => x.Username).Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

            RuleFor(x => x.Street).Length(5, 255).WithMessage("Street must be between 5 and 255 characters.");

            RuleFor(x => x.City).Length(2, 100).WithMessage("City must be between 2 and 100 characters.");

            RuleFor(x => x.District).Length(2, 100).WithMessage("District must be between 2 and 100 characters.");

            RuleFor(x => x.Ward).Length(2, 100).WithMessage("Ward must be between 2 and 100 characters.");

            RuleFor(x => x.ZipCode).Length(5, 20).WithMessage("Zip code must be between 5 and 20 characters.");

            RuleFor(x => x.PhoneNumber).Matches(@"^\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits.");
        }
    }
}