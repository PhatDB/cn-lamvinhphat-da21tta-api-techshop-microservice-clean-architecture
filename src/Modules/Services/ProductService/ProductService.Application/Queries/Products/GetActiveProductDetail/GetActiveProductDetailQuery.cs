using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetActiveProductDetail
{
    public record GetActiveProductDetailQuery(int ProductId) : IQuery<ProductDetailDTO>;
}