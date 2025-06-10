using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Domain.Abstractions.Repositories;
using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartService.Persistence.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Result<Cart>> GetCartAsync(
            int? customerId, string? sessionId, CancellationToken cancellationToken = default)
        {
            Cart? result = await _context.Carts.AsQueryable().Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId || c.SessionId == sessionId, cancellationToken);

            if (result == null)
                return Result.Failure<Cart>(Error.NotFound("Cart.NotFound", "Cart not found"));

            return Result.Success(result);
        }
    }
}