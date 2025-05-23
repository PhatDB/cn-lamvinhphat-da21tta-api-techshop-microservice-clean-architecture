using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CartService.Application.Commands.Cart.AddToCarts;
using CartService.Application.Commands.Cart.ClearCart;
using CartService.Application.Commands.Cart.RemoveItems;
using CartService.Application.DTOs;
using CartService.Application.Queries.Cart;
using MediatR;

namespace CartService.Api.Endpoint.Carts
{
    public class CartEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart/add-to-cart", async (AddToCartCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.Match(success => Results.Ok(new { Id = result.Value }),
                    failure => CustomResults.Problem(failure));
            }).WithName("CreateCart").WithTags("Cart");

            app.MapGet("/cart/{userId:int}", async (int userId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetCartByCustomerIdQuery query = new(userId);

                Result<CartDTO> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => Results.NotFound(failure.Error));
            }).WithName("GetCartByUserId").WithTags("Cart");

            app.MapPost("/cart/remove-item", async (RemoveItemCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("RemoveItem").WithTags("Cart");

            app.MapPost("/cart/clear", async (ClearCartCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("ClearCart").WithTags("Cart");
        }
    }
}