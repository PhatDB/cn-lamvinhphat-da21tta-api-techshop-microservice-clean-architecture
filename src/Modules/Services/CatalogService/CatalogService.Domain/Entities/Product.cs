using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;

namespace CatalogService.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        private Product()
        {
        }

        public string ProductName { get; private set; }
    }
}