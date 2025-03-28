using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.Orders.CancelOrder
{
    public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }


        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            Result<Order> orderResult = await _orderRepository.AsQueryable().Include(o => o.OrderItems).Where(o => o.Id == request.OrderId).FirstOrDefaultAsync(cancellationToken);

            if (orderResult.IsFailure)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order Not Found"));

            Order order = orderResult.Value;

            List<OrderItemDTO> orderItems = order.OrderItems.Select(oi => new OrderItemDTO { ProductId = oi.ProductId, Quantity = oi.Quantity }).ToList();

            if (order.SetCancelledStatus().IsFailure)
                return Result.Failure(order.SetCancelledStatus().Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            OrderCancel orderCancelEvent = new(orderItems);

            await _publishEndpoint.Publish(orderCancelEvent, cancellationToken);

            return Result.Success();
        }
    }
}