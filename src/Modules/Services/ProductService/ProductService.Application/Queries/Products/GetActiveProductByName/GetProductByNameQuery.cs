using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetActiveProductByName
{
    public record GetProductByNameQuery(string ProductName, PaginationOption PaginationOption)
        : IQuery<PagedResult<GetAllProductDTO>>;
}