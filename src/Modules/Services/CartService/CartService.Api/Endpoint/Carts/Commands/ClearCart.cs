using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CartService.Application.Commands.Cart.ClearCart;
using MediatR;

namespace CartService.Api.Endpoint.Carts.Commands
{
    public class ClearCart : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart/clear", async (ClearCartCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("ClearCart").WithTags("Cart");
        }
    }
}