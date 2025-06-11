using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetTotalProducts
{
    public record GetTotalProductsQuery : IQuery<TotalProductDto>;
}