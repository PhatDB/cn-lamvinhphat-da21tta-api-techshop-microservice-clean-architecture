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
            var cartInfo = await _cartRepository.AsQueryable().Where(c => c.UserId == context.Message.UserId).Select(c => new
            {
                c.Id,
                c.UserId,
                CartItems = c.CartItems.Select(item => new CartItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ProductName = item.ProductName,
                    ImgUrl = item.ImgUrl
                }).ToList()
            }).FirstOrDefaultAsync();

            if (cartInfo == null)
                await context.RespondAsync(new GetCartInfoResponse(0, 0, new List<CartItemDTO>()));
            else
                await context.RespondAsync(new GetCartInfoResponse(cartInfo.Id, cartInfo.UserId, cartInfo.CartItems));
        }
    }
}