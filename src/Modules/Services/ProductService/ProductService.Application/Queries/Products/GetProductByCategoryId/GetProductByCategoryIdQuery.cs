using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetProductByCategoryId
{
    public record GetProductByCategoryIdQuery(int CategoryId)
        : IQuery<List<GetAllProductDTO>>;
}