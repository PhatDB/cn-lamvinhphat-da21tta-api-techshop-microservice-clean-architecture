using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using MassTransit;
using OrderService.Application.Abstractions;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;
using OrderService.Domain.Enum;

namespace OrderService.Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderService orderService, IOrderRepository orderRepository, IPublishEndpoint publishEndpoint,
            IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Result<GetCartInfoResponse> cartResult = await _orderService.GetCartInfo(request.CartId);
            if (cartResult.IsFailure)
                return Result.Failure<int>(cartResult.Error);

            GetCartInfoResponse cart = cartResult.Value!;
            if (!cart.CartItems.Any())
                return Result.Failure<int>(Error.Validation("Cart.Empty", "Cart does not contain any items."));

            decimal totalAmount = cart.CartItems.Sum(ci => ci.Price * ci.Quantity);

            Result<Order> orderResult = Order.Create(cart.CustomerId, OrderStatus.AwaitingValidation, totalAmount,
                request.ReceiverName, request.ReceiverPhone, request.ReceiverAddress, request.Note, request.SessionId,
                request.PaymentMethod);

            if (orderResult.IsFailure)
                return Result.Failure<int>(orderResult.Error);

            Order order = orderResult.Value;

            foreach (CartItemDTO item in cart.CartItems)
                order.AddItem(item.ProductId, item.Quantity, item.Price);

            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            OrderCreated orderCreated = new(order.Id, order.CustomerId, request.CartId,
                cart.CartItems.Select(ci => new OrderItemInfo(ci.ProductId, ci.Quantity, ci.Price)).ToList());

            await _publishEndpoint.Publish(orderCreated, cancellationToken);

            return Result.Success(order.Id);
        }
    }
}