using CartService.Application.Commands.Cart.ClearCart;
using FluentValidation;

namespace CartService.Application.Validations.Cart
{
    public class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
    {
        public ClearCartCommandValidator()
        {
            RuleFor(command => command.CartId).GreaterThan(0).WithMessage("CartId must be greater than 0");
        }
    }
}