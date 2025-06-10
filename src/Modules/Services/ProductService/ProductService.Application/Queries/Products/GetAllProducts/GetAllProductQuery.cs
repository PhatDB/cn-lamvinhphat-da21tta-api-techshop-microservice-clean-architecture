using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetAllProducts
{
    public record GetAllProductQuery : IQuery<List<GetAllProductDTO>>;
}