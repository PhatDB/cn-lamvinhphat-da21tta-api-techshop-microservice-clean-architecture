using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using CartService.Application.Commands.Cart.RemoveItems;
using MediatR;

namespace CartService.Api.Endpoint.Carts.Commands
{
    public class RemoveItem : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart/remove-item", async (RemoveItemCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result.Error);
            }).WithName("RemoveItem").WithTags("Cart");
        }
    }
}