using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries.Dashboard
{
    public record GetDashboardStatsQuery : IQuery<DashboardStatsDto>;
}