using AutoMapper;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.DTOs;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Queries.Orders
{
    public class GetOrderByUserIdQueryHandler : IQueryHandler<GetOrderByUserIdQuery, OrderDTO>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByUserIdQueryHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderDTO>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            Order? orderResult = await _orderRepository.AsQueryable().Include(o => o.OrderItems).Where(o => o.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);

            if (orderResult == null)
                return Result.Failure<OrderDTO>(Error.NotFound("Order.NotFound", "Order not found"));

            OrderDTO orderDto = _mapper.Map<OrderDTO>(orderResult);

            return Result.Success(orderDto);
        }
    }
}