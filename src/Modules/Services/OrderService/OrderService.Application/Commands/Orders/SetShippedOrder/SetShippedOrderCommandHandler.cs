using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.Orders.SetShippedOrder
{
    public class SetShippedOrderCommandHandler : ICommandHandler<SetShippedOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetShippedOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetShippedOrderCommand request, CancellationToken cancellationToken)
        {
            Result<Order> orderResult = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (orderResult.IsFailure)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order Not Found"));

            Order order = orderResult.Value;

            if (order.SetShippedStatus().IsFailure)
                return Result.Failure(order.SetShippedStatus().Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}