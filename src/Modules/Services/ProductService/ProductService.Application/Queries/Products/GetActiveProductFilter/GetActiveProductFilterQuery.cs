using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetActiveProductFilter
{
    public record GetActiveProductFilterQuery(
        string? Keyword,
        int? CategoryId,
        int? BrandId,
        decimal? MinPrice,
        decimal? MaxPrice,
        bool? IsFeatured,
        PaginationOption PaginationOption) : IQuery<PagedResult<GetAllProductDTO>>;
}