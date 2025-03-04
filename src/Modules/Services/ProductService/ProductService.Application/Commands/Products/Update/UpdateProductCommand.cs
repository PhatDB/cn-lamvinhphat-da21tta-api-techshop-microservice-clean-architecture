using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.Products.Update
{
    public record UpdateProductCommand(
        int ProductId,
        string? Name,
        string? Sku,
        decimal? Price,
        int? CategoryId,
        int? SoldQuantity,
        bool? IsActive,
        List<ColorDTO>? Colors,
        string? Description,
        decimal? DiscountPrice) : ICommand;
}