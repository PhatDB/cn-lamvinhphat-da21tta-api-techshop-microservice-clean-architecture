using FluentValidation;
using ProductService.Application.Commands.Products.Update;

namespace ProductService.Application.Validations.Products
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0)
                .WithMessage("ProductId must be a valid ID.");

            RuleFor(x => x.Name).MaximumLength(255)
                .WithMessage("Product name must not exceed 255 characters.");

            RuleFor(x => x.Sku).MaximumLength(100)
                .WithMessage("SKU must not exceed 100 characters.");
        }
    }
}