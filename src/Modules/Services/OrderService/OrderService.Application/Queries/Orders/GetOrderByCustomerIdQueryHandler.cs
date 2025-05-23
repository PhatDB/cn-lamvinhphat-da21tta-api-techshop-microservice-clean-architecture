using AutoMapper;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions;
using OrderService.Application.DTOs;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Queries.Orders
{
    public class GetOrderByCustomerIdQueryHandler : IQueryHandler<GetOrderByCustomerIdQuery, OrderDTO>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public GetOrderByCustomerIdQueryHandler(
            IMapper mapper, IOrderRepository orderRepository, IOrderService orderService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderService = orderService;
        }

        public async Task<Result<OrderDTO>> Handle(
            GetOrderByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.AsQueryable().Where(o => o.CustomerId == request.CustomerId)
                .OrderByDescending(o => o.CreatedAt).FirstOrDefaultAsync(cancellationToken);

            if (order is null)
                return Result.Failure<OrderDTO>(Error.NotFound("Order.NotFound", "No order found for this customer."));

            List<int> productIds = order.OrderItems.Select(i => i.ProductId).Distinct().ToList();

            Result<GetListProductInfoRespone> productInfoResult = await _orderService.GetListProductInfo(productIds);
            if (productInfoResult.IsFailure)
                return Result.Failure<OrderDTO>(productInfoResult.Error);

            Dictionary<int, string> productMap =
                productInfoResult.Value.ProductInfos.ToDictionary(p => p.ProductId, p => p.ProductName);


            OrderDTO dto = _mapper.Map<OrderDTO>(order);

            foreach (OrderItemDTO item in dto.OrderItems)
                if (productMap.TryGetValue(item.ProductId, out string? name))
                    item.ProductName = name;

            return Result.Success(dto);
        }
    }
}