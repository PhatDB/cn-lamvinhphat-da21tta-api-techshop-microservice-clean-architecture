using FluentValidation;
using ProductService.Application.Commands.Products.Delete;

namespace ProductService.Application.Validations.Products
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0)
                .WithMessage("ProductId must be a valid ID.");
        }
    }
}