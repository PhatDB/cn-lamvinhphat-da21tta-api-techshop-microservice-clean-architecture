using FluentValidation;
using ProductService.Application.Commands.Products.AddImages;

namespace ProductService.Application.Validations.Images
{
    public class AddImageCommandValidator : AbstractValidator<AddImageCommand>
    {
        public AddImageCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0)
                .WithMessage("ProductId must be a valid ID.");

            RuleForEach(x => x.ProductImages).ChildRules(image =>
            {
                image.RuleFor(i => i.ImageContent).NotEmpty()
                    .WithMessage("Image content is required.");
            });
        }
    }
}