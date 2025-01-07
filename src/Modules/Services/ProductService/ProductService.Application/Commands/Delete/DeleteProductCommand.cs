using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Delete
{
    public record DeleteProductCommand : ICommand
    {
        public DeleteProductCommand(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; init; }
    }
}