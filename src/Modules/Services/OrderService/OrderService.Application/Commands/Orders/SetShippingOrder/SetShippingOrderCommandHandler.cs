using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using OrderService.Application.Commands.Orders.SetShippingOrder;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.Orders.SetShippedOrder
{
    public class SetShippingOrderCommandHandler : ICommandHandler<SetShippingOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetShippingOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetShippingOrderCommand request, CancellationToken cancellationToken)
        {
            Result<Order> orderResult = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (orderResult.IsFailure)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order Not Found"));

            Order order = orderResult.Value;

            if (order.SetShippingStatus().IsFailure)
                return Result.Failure(order.SetShippingStatus().Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}