using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using CartService.Application.Commands.Cart.AddToCarts;
using MediatR;

namespace CartService.Api.Endpoint.Carts.Commands
{
    public class AddToCart : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart/add-to-cart", async (AddToCartCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                if (result.IsSuccess)
                    return Results.Ok(new { CartId = result.Value });

                return Results.BadRequest(result.Error);
            }).WithName("CreateCart").WithTags("Cart");
        }
    }
}