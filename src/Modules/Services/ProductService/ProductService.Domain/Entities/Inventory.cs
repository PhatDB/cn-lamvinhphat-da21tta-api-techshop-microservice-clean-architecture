using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using BuildingBlocks.Error;

namespace ProductService.Domain.Entities
{
    public class Inventory : Entity, IAggregateRoot
    {
        public int ProductId { get; private set; }
        public int StockQuantity { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public Inventory(int productId, int stockQuantity)
        {
            ProductId = productId;
            StockQuantity = stockQuantity;
            LastUpdated = DateTime.UtcNow;
        }

        public Result UpdateStock(int quantity)
        {
            if (StockQuantity + quantity < 0)
                return Result.Failure(Error.Validation("Inventory.InsufficientStock", "Not enough stock."));

            StockQuantity += quantity;
            LastUpdated = DateTime.UtcNow;
            return Result.Success();
        }
    }
}