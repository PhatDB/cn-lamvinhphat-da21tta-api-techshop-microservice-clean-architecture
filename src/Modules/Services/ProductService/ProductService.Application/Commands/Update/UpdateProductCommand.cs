using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Update
{
    public record UpdateProductCommand(
        int ProductId,
        string Name,
        string Sku,
        decimal Price,
        int CategoryId,
        string? Description = null,
        decimal? DiscountPrice = null
    ) : ICommand;
}