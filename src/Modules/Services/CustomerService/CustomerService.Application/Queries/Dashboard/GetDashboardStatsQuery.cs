using BuildingBlocks.CQRS;
using CustomerService.Application.DTOs;

namespace CustomerService.Application.Queries.Dashboard
{
    public record GetDashboardStatsQuery : IQuery<DashboardStatsDto>;
}