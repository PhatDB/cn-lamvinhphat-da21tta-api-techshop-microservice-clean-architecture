using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Orders;
using CartService.Domain.Abstractions.Repositories;
using CartService.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Consumers
{
    public class OrderSummitConsumer : IConsumer<OrderSummit>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderSummitConsumer(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderSummit> context)
        {
            Cart? cartResult = await _cartRepository.AsQueryable().Include(c => c.CartItems).Where(c => c.UserId == context.Message.UserId).FirstOrDefaultAsync();

            await _cartRepository.DeleteAsync(cartResult);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}