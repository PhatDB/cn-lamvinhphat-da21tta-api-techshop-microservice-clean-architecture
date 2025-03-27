using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using MassTransit;
using OrderService.Application.Abstractions;
using OrderService.Domain.Abstractions.Repositories;
using Order = OrderService.Domain.Entities.Order;

namespace OrderService.Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderService orderService, IOrderRepository orderRepository, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Result<GetCartInfoResponse> cartResult = await _orderService.GetCartInfo(request.UserId);
            if (cartResult.IsFailure)
                return Result.Failure<int>(cartResult.Error);

            GetCartInfoResponse cart = cartResult.Value;

            Result userResult = await _orderService.IsUserExist(request.UserId);
            if (userResult.IsFailure)
                return Result.Failure<int>(userResult.Error);

            Order order = new(request.UserId, request.Street, request.City, request.District, request.Ward, request.ZipCode, request.PhoneNumber);

            foreach (CartItemDTO cartItem in cart.CartItems)

                order.AddItem(cartItem.ProductId, cartItem.Quantity, cartItem.UnitPrice);

            await _orderRepository.AddAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(order.Id);
        }
    }
}