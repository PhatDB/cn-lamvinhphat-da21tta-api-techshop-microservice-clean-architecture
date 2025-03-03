using FluentValidation;
using ProductService.Application.Commands.Products.DeleteImages;

namespace ProductService.Application.Validations.Images
{
    public class DeleteImageCommandValidator : AbstractValidator<DeleteImageCommand>
    {
        public DeleteImageCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0)
                .WithMessage("ProductId must be a valid ID.");
            RuleFor(x => x.ImageIds).NotEmpty()
                .WithMessage("At least one image ID must be provided.");
            RuleForEach(x => x.ImageIds).GreaterThan(0)
                .WithMessage("Image ID must be a valid ID.");
        }
    }
}