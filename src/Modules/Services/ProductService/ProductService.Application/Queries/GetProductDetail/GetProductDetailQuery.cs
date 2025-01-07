using BuildingBlocks.CQRS;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public class GetProductDetailQuery : IQuery<Product>
    {
        public GetProductDetailQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; }
    }
}