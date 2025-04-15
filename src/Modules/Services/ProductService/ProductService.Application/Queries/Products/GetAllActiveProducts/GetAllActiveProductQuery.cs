using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetAllActiveProducts
{
    public record GetAllActiveProductQuery(PaginationOption PaginationOption) : IQuery<PagedResult<GetAllProductDTO>>;
}