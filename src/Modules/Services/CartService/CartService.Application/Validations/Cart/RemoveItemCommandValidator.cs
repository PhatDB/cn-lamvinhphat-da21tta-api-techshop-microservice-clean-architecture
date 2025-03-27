using CartService.Application.Commands.Cart.RemoveItems;
using FluentValidation;

namespace CartService.Application.Validations.Cart
{
    public class RemoveItemCommandValidator : AbstractValidator<RemoveItemCommand>
    {
        public RemoveItemCommandValidator()
        {
            RuleFor(command => command.CartId).GreaterThan(0).WithMessage("CartId must be greater than 0");

            RuleFor(command => command.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0");
        }
    }
}