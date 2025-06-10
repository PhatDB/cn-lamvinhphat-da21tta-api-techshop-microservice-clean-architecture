using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Results;
using CartService.Domain.Entities;

namespace CartService.Domain.Abstractions.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Result<Cart>> GetCartAsync(
            int? customerId, string? sessionId, CancellationToken cancellationToken = default);
    }
}