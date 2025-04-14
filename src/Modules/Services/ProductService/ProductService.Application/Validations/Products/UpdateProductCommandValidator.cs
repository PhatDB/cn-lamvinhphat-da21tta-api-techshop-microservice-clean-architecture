using FluentValidation;
using ProductService.Application.Commands.Products.Update;

namespace ProductService.Application.Validations.Products
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
    }
}