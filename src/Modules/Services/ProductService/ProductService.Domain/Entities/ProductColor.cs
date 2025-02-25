using BuildingBlocks.Abstractions.Entities;

namespace ProductService.Domain.Entities
{
    public class ProductColor : Entity
    {
        public int ProductId { get; private set; }
        public int ColorId { get; private set; }

        public ProductColor(int productId, int colorId)
        {
            ProductId = productId;
            ColorId = colorId;
        }
    }
}