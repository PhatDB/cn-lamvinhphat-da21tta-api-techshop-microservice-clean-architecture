using FluentValidation;
using ProductService.Application.Commands.Products.Update;

namespace ProductService.Application.Validations.Products
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId is required and must be greater than 0.");

            RuleFor(x => x.ProductName).MaximumLength(200).WithMessage("Product name must not exceed 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ProductName));

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0.")
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.BrandId).GreaterThan(0).WithMessage("BrandId must be greater than 0.")
                .When(x => x.BrandId.HasValue);

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount must be >= 0.")
                .When(x => x.Discount.HasValue);

            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock must be >= 0.")
                .When(x => x.Stock.HasValue);

            RuleFor(x => x.SoldQuantity).GreaterThanOrEqualTo(0).WithMessage("Sold quantity must be >= 0.")
                .When(x => x.SoldQuantity.HasValue);

            RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleForEach(x => x.NewImages).ChildRules(images =>
            {
                images.RuleFor(i => i.ImageContent).NotEmpty().WithMessage("Image content must not be empty.");

                images.RuleFor(i => i.SortOrder).GreaterThanOrEqualTo(0).WithMessage("Sort order must be >= 0.");
            }).When(x => x.NewImages is not null && x.NewImages.Any());

            RuleFor(x => x.NewImages.Count(i => i.IsMain)).LessThanOrEqualTo(1)
                .WithMessage("Only one image can be marked as main.")
                .When(x => x.NewImages is not null && x.NewImages.Any());
        }
    }
}