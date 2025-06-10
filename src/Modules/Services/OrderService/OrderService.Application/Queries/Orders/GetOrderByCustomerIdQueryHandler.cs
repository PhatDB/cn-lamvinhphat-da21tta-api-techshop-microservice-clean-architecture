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
    public class GetOrderByCustomerIdQueryHandler : IQueryHandler<GetOrderByCustomerIdQuery, List<OrderDTO>>
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

        public async Task<Result<List<OrderDTO>>> Handle(
            GetOrderByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            List<Order> orders = await _orderRepository.AsQueryable().Include(o => o.OrderItems)
                .Where(o => o.CustomerId == request.CustomerId).OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

            if (!orders.Any())
                return Result.Failure<List<OrderDTO>>(Error.NotFound("Order.NotFound",
                    "No order found for this customer."));

            List<int> productIds = orders.SelectMany(o => o.OrderItems).Select(i => i.ProductId).Distinct().ToList();

            Result<GetListProductInfoResponse> productInfoResult = await _orderService.GetListProductInfo(productIds);
            if (productInfoResult.IsFailure)
                return Result.Failure<List<OrderDTO>>(productInfoResult.Error);

            Dictionary<int, string> productMap =
                productInfoResult.Value.ProductInfos.ToDictionary(p => p.ProductId, p => p.ProductName);

            List<OrderDTO> dtoList = _mapper.Map<List<OrderDTO>>(orders);

            foreach (OrderDTO orderDto in dtoList)
            foreach (OrderItemDTO item in orderDto.OrderItems)
                if (productMap.TryGetValue(item.ProductId, out string? name))
                    item.ProductName = name;

            return Result.Success(dtoList);
        }
    }
}