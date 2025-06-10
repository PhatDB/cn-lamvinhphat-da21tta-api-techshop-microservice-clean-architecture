using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.Orders.SetDeliveredOrder
{
    public class SetDeliveredOrderCommandHandler : ICommandHandler<SetDeliveredOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetDeliveredOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetDeliveredOrderCommand request, CancellationToken cancellationToken)
        {
            Result<Order> orderResult = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (orderResult.IsFailure)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order Not Found"));

            Order order = orderResult.Value;

            if (order.SetPaidStatus().IsFailure)
                return Result.Failure(order.SetDeliveredStatus().Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}