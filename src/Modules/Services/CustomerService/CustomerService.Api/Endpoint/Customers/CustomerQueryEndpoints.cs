using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CustomerService.Application.DTOs;
using CustomerService.Application.Queries.Customers;
using CustomerService.Application.Queries.Dashboard;
using CustomerService.Application.Queries.Reviews;
using MediatR;

namespace CustomerService.Api.Endpoint.Customers
{
    public class CustomerQueryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/customers/{customerId:int}", async (int customerId, ISender sender) =>
            {
                GetCustomerInfoQuery query = new(customerId);
                Result<CustomerDto> result = await sender.Send(query);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithName("GetCustomerInfo").WithTags("Customer");

            app.MapGet("/customers", async (ISender sender) =>
            {
                GetAllCustomerInfoQuery query = new();
                Result<List<CustomerDto>> result = await sender.Send(query);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithName("GetAllCustomerInfo").WithTags("Customer");

            app.MapGet("/customers/dashboard", async (ISender sender) =>
            {
                GetDashboardStatsQuery query = new();
                Result<DashboardStatsDto> result = await sender.Send(query);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithName("GetDashboard").WithTags("Customer");

            app.MapGet("/customers/reviews", async (ISender sender) =>
            {
                GetAllReviewsQuery query = new();
                Result<List<ReviewDto>> result = await sender.Send(query);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithName("GetAllReviews").WithTags("Customer");
        }
    }
}