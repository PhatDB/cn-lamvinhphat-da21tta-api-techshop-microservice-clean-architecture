using BuildingBlocks.Abstractions.Repository;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Abstractions.Repositories
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        
    }
}