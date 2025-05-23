using CartService.Application.Commands.Cart.AddToCarts;
using FluentValidation;

namespace CartService.Application.Validations.Cart
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(command => command.CustomerId).GreaterThan(0).WithMessage("UserId must be greater than 0");

            RuleFor(command => command.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0");

            RuleFor(command => command.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}