using BuildingBlocks.Abstractions.Repository;
using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Abstractions.Repositories
{
    public interface ICategoryRepo : IGenericRepository<Category>
    {
    }
}