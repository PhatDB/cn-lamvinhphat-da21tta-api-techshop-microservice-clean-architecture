using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
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

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithName("CreateCart").WithTags("Cart");
        }
    }
}