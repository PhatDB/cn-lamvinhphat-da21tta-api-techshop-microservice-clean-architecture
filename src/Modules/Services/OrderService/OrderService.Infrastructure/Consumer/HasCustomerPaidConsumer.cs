using BuildingBlocks.Contracts.Customers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Enum;

namespace OrderService.Infrastructure.Consumer
{
    public class HasCustomerPaidConsumer : IConsumer<HasCustomerPaidRequest>
    {
        private readonly IOrderRepository _orderRepository;

        public HasCustomerPaidConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<HasCustomerPaidRequest> context)
        {
            int customerId = context.Message.CustomerId;

            bool hasCustomerPaid = await _orderRepository.AsQueryable().AsNoTracking()
                .AnyAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.Paid);

            await context.RespondAsync(new HasCustomerPaidResponse(hasCustomerPaid));
        }
    }
}