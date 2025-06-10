using AutoMapper;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions;
using OrderService.Application.DTOs;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Queries.Orders
{
    public class GetOrderDetailQueryHandler : IQueryHandler<GetOrderDetailQuery, OrderDTO>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public GetOrderDetailQueryHandler(IMapper mapper, IOrderRepository orderRepository, IOrderService orderService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderService = orderService;
        }


        public async Task<Result<OrderDTO>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.AsQueryable().Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            List<int> productIds = order.OrderItems.Select(o => o.ProductId).ToList();


            Result<GetListProductInfoResponse> productInfoResult = await _orderService.GetListProductInfo(productIds);
            if (productInfoResult.IsFailure)
                return Result.Failure<OrderDTO>(productInfoResult.Error);

            Dictionary<int, string> productMap =
                productInfoResult.Value.ProductInfos.ToDictionary(p => p.ProductId, p => p.ProductName);

            OrderDTO? dto = _mapper.Map<OrderDTO>(order);

            foreach (OrderItemDTO item in dto.OrderItems)
                if (productMap.TryGetValue(item.ProductId, out string? name))
                    item.ProductName = name;

            return Result.Success(dto);
        }
    }
}