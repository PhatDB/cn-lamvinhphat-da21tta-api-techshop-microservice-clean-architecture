using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using OrderService.Application.Commands.Orders.CreateOrder;

namespace OrderService.Api.Endpoint.Orders
{
    public class CreateOrder : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/create", async (CreateOrderCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.Match(success => Results.Ok(new { OrderId = success }), failure => Results.BadRequest(new { failure.Error }));
            }).WithName("CreateOrder").WithTags("Orders");
        }
    }
}