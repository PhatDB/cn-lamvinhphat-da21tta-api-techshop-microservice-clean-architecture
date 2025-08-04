using System.Text;
using System.Text.Json;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using OrderService.Application.Commands.Orders.CancelOrder;
using OrderService.Application.Commands.Orders.CreateOrder;
using OrderService.Application.Commands.Orders.PayOS;
using OrderService.Application.Commands.Orders.SetConfirmedOrder;
using OrderService.Application.Commands.Orders.SetDeliveredOrder;
using OrderService.Application.Commands.Orders.SetPaidOrder;
using OrderService.Application.Commands.Orders.SetShippingOrder;
using OrderService.Application.DTOs;
using OrderService.Application.Queries.Dashboard;
using OrderService.Application.Queries.Orders;
using OrderService.Application.Queries.Orders.GetAllOrder;
using OrderService.Application.Queries.Orders.GetOrderDetail;

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

            app.MapPost("/order/payos/create-link", async (CreateLinkCommand command, ISender sender) =>
            {
                Result<string> result = await sender.Send(command);

                return result.Match(checkoutUrl => Results.Ok(new { checkoutUrl }),
                    error => CustomResults.Problem(error));
            }).WithName("CreatePayOsLink").WithTags("Payments");

            app.MapPost("/order/payos/webhook", async (HttpRequest req, ISender sender) =>
            {
                req.EnableBuffering();
                string rawBody;
                using (StreamReader reader = new(req.Body, Encoding.UTF8, leaveOpen: true))
                {
                    rawBody = await reader.ReadToEndAsync();
                    req.Body.Position = 0;
                }

                // 2) Parse JSON, tách data và signature
                using JsonDocument doc = JsonDocument.Parse(rawBody);
                JsonElement root = doc.RootElement;
                JsonElement dataEl = root.GetProperty("data");
                string signature = root.GetProperty("signature").GetString() ?? "";

                // 3) Dispatch CQRS command
                WebHookCommand cmd = new(dataEl, signature);
                Result result = await sender.Send(cmd);

                return result.IsSuccess ? Results.Ok() : Results.Problem(result.Error.ToString());
            }).AllowAnonymous().WithName("PayOsWebhook").WithTags("Payments");


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

            app.MapGet("/orders/dashboard", async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetDashboardStatsQuery query = new();
                Result<DashboardStatsDto> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Orders").WithName("GetDashboardStats");
        }
    }
}