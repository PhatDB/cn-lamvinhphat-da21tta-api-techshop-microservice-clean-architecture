using BuildingBlocks.Abstractions.Entities;

namespace ProductService.Domain.Entities
{
    public class ProductColor : Entity
    {
        public ProductColor(int productId, int colorId, int stockQuantity)
        {
            ProductId = productId;
            ColorId = colorId;
            StockQuantity = stockQuantity;
        }

        public int ProductId { get; private set; }
        public int ColorId { get; private set; }
        public int StockQuantity { get; private set; }

        public virtual Color Color { get; }

        public void UpdateStock(int? quantity)
        {
            StockQuantity = quantity ?? StockQuantity;
        }
    }
}