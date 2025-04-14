using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.Products.Create
{
    public record CreateProductCommand(
        string ProductName,
        int CategoryId,
        int BrandId,
        decimal Price,
        decimal? Discount,
        int Stock,
        string? Description,
        string? Specs,
        List<ProductImageDto> Images) : ICommand<int>;
}