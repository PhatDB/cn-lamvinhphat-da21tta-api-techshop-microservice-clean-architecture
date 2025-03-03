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

            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(255)
                .WithMessage("Product name must not exceed 255 characters.");

            RuleFor(x => x.Sku).NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId).GreaterThan(0)
                .WithMessage("CategoryId must be a valid ID.");
        }
    }
}