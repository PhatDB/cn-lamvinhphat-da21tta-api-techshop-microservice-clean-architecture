using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries
{
    public class GetProductDetailQuery : IQuery<ProductDetailDTO>
    {
        public GetProductDetailQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; }
    }
}