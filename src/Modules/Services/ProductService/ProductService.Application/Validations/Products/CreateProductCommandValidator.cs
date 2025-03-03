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
                .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId).GreaterThan(0)
                .WithMessage("CategoryId must be a valid ID.");

            RuleFor(x => x.Images).NotEmpty()
                .WithMessage("At least one image is required.")
                .Must(images => images.Any())
                .WithMessage("At least one image must be provided.");

            RuleForEach(x => x.Images).ChildRules(image =>
            {
                image.RuleFor(i => i.ImageContent).NotEmpty()
                    .WithMessage("Image content is required.");
            });

            RuleFor(x => x.Colors).NotEmpty()
                .WithMessage("At least one color is required.")
                .Must(colors => colors.Any())
                .WithMessage("At least one color must be provided.");

            RuleForEach(x => x.Colors).NotEmpty()
                .WithMessage("Color name cannot be empty.");
        }
    }
}