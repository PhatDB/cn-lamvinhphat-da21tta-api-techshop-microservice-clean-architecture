using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Products.Delete
{
    public record DeleteProductCommand(int ProductId) : ICommand;
}