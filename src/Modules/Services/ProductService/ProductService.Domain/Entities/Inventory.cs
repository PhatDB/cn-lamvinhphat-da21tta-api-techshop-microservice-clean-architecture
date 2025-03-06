using BuildingBlocks.Abstractions.Entities;

namespace ProductService.Domain.Entities
{
    public class Inventory : Entity
    {
        private Inventory(int productId, int stockQuantity)
        {
            ProductId = productId;
            StockQuantity = stockQuantity;
            LastUpdated = DateTime.UtcNow;
        }

        private Inventory()
        {
        }

        public int ProductId { get; private set; }
        public int StockQuantity { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public Product Product { get; }

        public static Inventory Create(int productId, int stockQuantity = 0)
        {
            return new Inventory(productId, stockQuantity);
        }

        public void UpdateStock(int? quantity)
        {
            StockQuantity = quantity ?? StockQuantity;
            LastUpdated = DateTime.UtcNow;
        }
    }
}