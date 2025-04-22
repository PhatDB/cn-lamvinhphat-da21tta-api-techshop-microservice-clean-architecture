using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetActiveProductByBrandId
{
    public record GetActiveProductByBrandIdQuery(int BrandId, PaginationOption PaginationOption)
        : IQuery<PagedResult<GetAllProductDTO>>;
}