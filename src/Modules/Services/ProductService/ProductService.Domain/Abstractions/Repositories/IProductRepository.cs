using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Abstractions.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Result<PagedResult<Product>>> GetAllPagedAsync(
            PaginationOption paginationOption, CancellationToken cancellationToken);

        Task<Result<Product>> GetProductDetailAsync(
            int id, CancellationToken cancellationToken = default);
    }
}