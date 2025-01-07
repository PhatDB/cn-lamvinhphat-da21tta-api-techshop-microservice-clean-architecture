using BuildingBlocks.Abstractions.Repository;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Abstractions.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool? isDescending, CancellationToken cancellationToken);
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    }
}