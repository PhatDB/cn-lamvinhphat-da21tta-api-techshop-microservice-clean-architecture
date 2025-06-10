using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CustomerService.Application.DTOs;
using CustomerService.Application.Queries.Customers;
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
        }
    }
}