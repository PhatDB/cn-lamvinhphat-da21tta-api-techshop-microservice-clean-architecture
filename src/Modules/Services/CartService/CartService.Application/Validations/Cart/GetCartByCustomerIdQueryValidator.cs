using CartService.Application.Queries.Cart;
using FluentValidation;

namespace CartService.Application.Validations.Cart
{
    public class GetCartByCustomerIdQueryValidator : AbstractValidator<GetCartByCustomerIdQuery>
    {
        public GetCartByCustomerIdQueryValidator()
        {
            RuleFor(query => query.CustomerId).GreaterThan(0).WithMessage("UserId must be greater than 0");
        }
    }
}