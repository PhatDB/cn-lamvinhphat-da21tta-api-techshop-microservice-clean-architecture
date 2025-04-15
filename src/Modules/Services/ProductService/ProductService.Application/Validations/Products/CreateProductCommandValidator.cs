using FluentValidation;
using ProductService.Application.Commands.Products.Create;

namespace ProductService.Application.Validations.Products
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.").MaximumLength(200)
                .WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0.");

            RuleFor(x => x.BrandId).GreaterThan(0).WithMessage("BrandId must be greater than 0.");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount must be >= 0.")
                .LessThanOrEqualTo(x => x.Price).WithMessage("Discount must not exceed price.");

            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock must be >= 0.");

            RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Specs).MaximumLength(2000).WithMessage("Specs must not exceed 2000 characters.");

            RuleFor(x => x.Images).NotNull().WithMessage("Images are required.").Must(x => x.Any())
                .WithMessage("At least one image is required.");

            RuleForEach(x => x.Images).ChildRules(images =>
            {
                images.RuleFor(i => i.ImageContent).NotEmpty().WithMessage("Image content must not be empty.");

                images.RuleFor(i => i.SortOrder).GreaterThanOrEqualTo(0).WithMessage("Sort order must be >= 0.");
            });

            RuleFor(x => x.Images.Count(i => i.IsMain)).Equal(1)
                .WithMessage("Exactly one image must be marked as main.");
        }
    }
}