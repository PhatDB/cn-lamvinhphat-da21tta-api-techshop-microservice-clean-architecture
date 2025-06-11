using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.DTOs;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;
using OrderService.Domain.Enum;

namespace OrderService.Application.Queries.Dashboard
{
    public class GetDashboardStatsQueryHandler : IQueryHandler<GetDashboardStatsQuery, DashboardStatsDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetDashboardStatsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<DashboardStatsDto>> Handle(
            GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime startOfMonth = new(today.Year, today.Month, 1);

            List<Order> monthlyOrders = await _orderRepository.AsQueryable().AsNoTracking()
                .Where(o => o.CreatedAt >= startOfMonth && o.Status == OrderStatus.Paid).ToListAsync(cancellationToken);

            double monthlyRevenue = monthlyOrders.Sum(o => (double)o.TotalAmount);

            List<Order> todayOrders = monthlyOrders.Where(o => o.CreatedAt.Date == today).ToList();

            double dailyRevenue = todayOrders.Sum(o => (double)o.TotalAmount);
            int monthlySales = monthlyOrders.Count;
            int dailySales = todayOrders.Count;

            DashboardStatsDto dto = new()
            {
                MonthlyRevenue = monthlyRevenue,
                DailyRevenue = dailyRevenue,
                MonthlySales = monthlySales,
                DailySales = dailySales
            };

            return Result.Success(dto);
        }
    }
}