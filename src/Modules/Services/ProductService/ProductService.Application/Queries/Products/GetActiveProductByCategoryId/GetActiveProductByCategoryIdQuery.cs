using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetActiveProductByCategoryId
{
    public record GetActiveProductByCategoryIdQuery(int CategoryId, PaginationOption PaginationOption)
        : IQuery<PagedResult<GetAllProductDTO>>;
}