using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Results;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Abstractions.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool? isDescending, CancellationToken cancellationToken);
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
        Task<Result> AddColorAsync(Color color, CancellationToken cancellationToken);
        Task<Result<Product>> GetProductDetailAsync(int id, CancellationToken cancellationToken = default);
    }
}