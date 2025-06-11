using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Queries.Dashboard
{
    public class GetDashboardStatsQueryHandler : IQueryHandler<GetDashboardStatsQuery, DashboardStatsDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetDashboardStatsQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<DashboardStatsDto>> Handle(
            GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            List<Customer> result = await _customerRepository.AsQueryable().AsNoTracking().Include(x => x.Reviews)
                .ToListAsync(cancellationToken);

            int totalUsers = result.Count();
            int totalReviews = result.Sum(x => x.Reviews.Count());

            DashboardStatsDto stats = new();
            stats.TotalCustomers = totalUsers;
            stats.TotalReviews = totalReviews;

            return Result<DashboardStatsDto>.Success(stats);
        }
    }
}