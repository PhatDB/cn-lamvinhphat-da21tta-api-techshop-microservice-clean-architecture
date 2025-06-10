using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using OrderService.Application.Commands.Orders.CancelOrder;
using OrderService.Application.Commands.Orders.CreateOrder;
using OrderService.Application.Commands.Orders.SetComfirmedOrder;
using OrderService.Application.Commands.Orders.SetDeliveredOrder;
using OrderService.Application.Commands.Orders.SetPaidOrder;
using OrderService.Application.Commands.Orders.SetShippingOrder;
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

            app.MapPost("/order/shipping", async (SetShippingOrderCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("ShippingOrder").WithTags("Orders");

            app.MapPost("/order/confirmed", async (SetConfirmedOrderCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("ConfirmedOrder").WithTags("Orders");

            app.MapPost("/order/delivered", async (SetDeliveredOrderCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("DeliveredOrder").WithTags("Orders");

            app.MapGet("/orders/customer/{customerId}", async (
                int customerId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetOrderByCustomerIdQuery query = new(customerId);
                Result<List<OrderDTO>> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Orders").WithName("GetOrderByUserId");

            app.MapGet("/orders", async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllOrderQuery query = new();
                Result<List<OrderDTO>> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Orders").WithName("GetAllOrders");

            app.MapGet("/orders/{orderId}", async (int orderId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetOrderDetailQuery query = new(orderId);
                Result<OrderDTO> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Orders").WithName("GetOrderDetail");
        }
    }
}