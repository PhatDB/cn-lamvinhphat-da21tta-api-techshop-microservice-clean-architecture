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
    public class GetAllOrderQueryHandler : IQueryHandler<GetAllOrderQuery, List<OrderDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public GetAllOrderQueryHandler(IMapper mapper, IOrderRepository orderRepository, IOrderService orderService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderService = orderService;
        }


        public async Task<Result<List<OrderDTO>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            List<Order> orders = await _orderRepository.AsQueryable().Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt).ToListAsync(cancellationToken);

            List<int> productIds = orders.SelectMany(o => o.OrderItems).Select(oi => oi.ProductId).Distinct().ToList();

            Result<GetListProductInfoResponse> productInfoResult = await _orderService.GetListProductInfo(productIds);

            Dictionary<int, ProductInfoResponse> productInfoDict =
                productInfoResult.Value.ProductInfos.ToDictionary(p => p.ProductId, p => p);

            List<OrderDTO> mapped = _mapper.Map<List<OrderDTO>>(orders);

            foreach (OrderDTO order in mapped)
            foreach (OrderItemDTO item in order.OrderItems)
                if (productInfoDict.TryGetValue(item.ProductId, out ProductInfoResponse? productInfo))
                    item.ProductName = productInfo.ProductName;

            return Result.Success(mapped);
        }
    }
}