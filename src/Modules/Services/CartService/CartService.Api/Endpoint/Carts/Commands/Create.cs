using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CartService.Application.Commands.Cart.Create;
using MediatR;

namespace CartService.Api.Endpoint.Carts.Commands
{
    public class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart/create", async (CreateCartCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.Match(value => Results.Ok(new { CartId = value }), error => Results.BadRequest(error));
            }).WithName("CreateCart").WithTags("Cart");
        }
    }
}