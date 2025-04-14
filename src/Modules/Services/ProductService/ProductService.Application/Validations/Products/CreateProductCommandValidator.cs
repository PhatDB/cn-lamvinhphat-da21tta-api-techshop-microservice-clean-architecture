using FluentValidation;
using ProductService.Application.Commands.Products.Create;

namespace ProductService.Application.Validations.Products
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
    }
}