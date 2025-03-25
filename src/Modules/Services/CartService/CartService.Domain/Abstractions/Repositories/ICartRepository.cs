using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Results;
using CartService.Domain.Entities;

namespace CartService.Domain.Abstractions.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Result<Cart>> GetUserCartAsync(int userId, CancellationToken cancellationToken = default);
    }
}