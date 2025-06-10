using BuildingBlocks.Contracts.Carts;
using CartService.Domain.Abstractions.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Consumers
{
    public class GetCartInfoConsumer : IConsumer<GetCartInfo>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartInfoConsumer(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Consume(ConsumeContext<GetCartInfo> context)
        {
            var cartInfo = await _cartRepository.AsQueryable().Include(c => c.CartItems)
                .Where(c => c.Id == context.Message.CartId).Select(c => new
                {
                    c.Id,
                    c.CustomerId,
                    CartItems = c.CartItems.Select(item => new CartItemDTO
                    {
                        ProductId = item.ProductId, Quantity = item.Quantity, Price = item.Price
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (cartInfo == null)
            {
                await context.RespondAsync(new GetCartInfoResponse(0, 0, new List<CartItemDTO>()));
                return;
            }

            await context.RespondAsync(new GetCartInfoResponse(cartInfo.Id, cartInfo.CustomerId.Value,
                cartInfo.CartItems));
        }
    }
}