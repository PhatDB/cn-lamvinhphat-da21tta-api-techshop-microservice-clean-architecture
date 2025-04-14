using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.Products.Update
{
    public record UpdateProductCommand(
        int ProductId,
        string? ProductName,
        decimal? Price,
        int? Stock,
        int? CategoryId,
        int? BrandId,
        decimal? Discount,
        int? SoldQuantity,
        bool? IsActive,
        bool? IsFeatured,
        string? Description,
        string? Specs,
        List<ProductImageDto>? NewImages = null,
        List<int>? ImageIdsToRemove = null) : ICommand;
}