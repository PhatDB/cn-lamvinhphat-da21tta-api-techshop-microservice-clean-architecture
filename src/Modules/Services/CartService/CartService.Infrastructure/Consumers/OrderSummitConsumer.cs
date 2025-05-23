using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Orders;
using CartService.Domain.Abstractions.Repositories;
using CartService.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderCreatedConsumer(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            int cartId = context.Message.CartId;

            Cart? cart = await _cartRepository.AsQueryable().Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart != null)
            {
                await _cartRepository.DeleteAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}