using CartService.Application.Queries.Cart;
using FluentValidation;

namespace CartService.Application.Validations.Cart
{
    public class GetCartByUserIdQueryValidator : AbstractValidator<GetCartByUserIdQuery>
    {
        public GetCartByUserIdQueryValidator()
        {
            RuleFor(query => query.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0");
        }
    }
}