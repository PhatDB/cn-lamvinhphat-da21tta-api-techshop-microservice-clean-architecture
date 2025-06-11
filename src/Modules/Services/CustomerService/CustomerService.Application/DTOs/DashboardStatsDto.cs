namespace CustomerService.Application.DTOs
{
    public record DashboardStatsDto
    {
        public int TotalCustomers { get; set; }
        public int TotalReviews { get; set; }
    }
}