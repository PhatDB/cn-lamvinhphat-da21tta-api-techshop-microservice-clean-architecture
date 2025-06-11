namespace OrderService.Application.DTOs
{
    public record DashboardStatsDto
    {
        public double MonthlyRevenue { get; set; }
        public double DailyRevenue { get; set; }
        public int MonthlySales { get; set; }
        public int DailySales { get; set; }
    }
}