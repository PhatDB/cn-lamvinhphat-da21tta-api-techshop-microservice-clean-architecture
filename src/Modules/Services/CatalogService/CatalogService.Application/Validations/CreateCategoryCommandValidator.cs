using CatalogService.Application.Commands.Create;
using FluentValidation;

namespace CatalogService.Application.Validations
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(c=>c.CategoryName).NotEmpty().NotNull();
        }
    }
}