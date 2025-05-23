using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using OrderService.Application.Commands.Orders.CancelOrder;
using OrderService.Application.Commands.Orders.CreateOrder;
using OrderService.Application.Commands.Orders.SetPaidOrder;
using OrderService.Application.Commands.Orders.SetShippedOrder;
using OrderService.Application.DTOs;
using OrderService.Application.Queries.Orders;

namespace OrderService.Api.Endpoint.Orders
{
    public class OrderEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/cancel", async (CancelOrderCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("CancelOrder").WithTags("Orders");

            app.MapPost("/order/create", async (CreateOrderCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.Match(success => Results.Ok(new { OrderId = success }),
                    failure => Results.BadRequest(new { failure.Error }));
            }).WithName("CreateOrder").WithTags("Orders");

            app.MapPost("/order/paid", async (SetPaidOrderCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("PaidOrder").WithTags("Orders");

            app.MapPost("/order/shipped", async (SetShippedOrderCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("ShippedOrder").WithTags("Orders");

            app.MapGet("/orders/{userId}", async (int userId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetOrderByCustomerIdQuery query = new(userId);
                Result<OrderDTO> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Orders").WithName("GetOrderByUserId");
        }
    }
}