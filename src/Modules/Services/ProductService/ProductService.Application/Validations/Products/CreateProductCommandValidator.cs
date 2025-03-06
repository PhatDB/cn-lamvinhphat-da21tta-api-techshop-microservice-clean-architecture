using FluentValidation;
using ProductService.Application.Commands.Products.Create;

namespace ProductService.Application.Validations.Products
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(255)
                .WithMessage("Product name must not exceed 255 characters.");

            RuleFor(x => x.SKU).NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.")
                .Matches("^[a-zA-Z0-9-_]+$").WithMessage(
                    "SKU can only contain letters, numbers, dashes, and underscores.");

            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId).GreaterThan(0)
                .WithMessage("CategoryId must be a valid ID.");

            RuleFor(x => x.Inventory).ChildRules(inventory =>
            {
                inventory.RuleFor(i => i.StockQuantity).NotEmpty().GreaterThan(0)
                    .WithMessage("Stock quantity must be greater than zero.");
            });

            RuleForEach(x => x.Images).ChildRules(image =>
            {
                image.RuleFor(i => i.ImageContent).NotEmpty()
                    .WithMessage("Image content is required.");
            });
        }
    }
}