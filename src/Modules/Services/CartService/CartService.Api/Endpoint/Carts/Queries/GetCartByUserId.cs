using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CartService.Application.DTOs;
using CartService.Application.Queries.Cart;
using MediatR;

namespace CartService.Api.Endpoint.Carts.Queries
{
    public class GetCartByUserId : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/cart/{userId:int}", async (int userId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetCartByUserIdQuery query = new(userId);

                Result<CartDTO> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => Results.NotFound(failure.Error));
            }).WithName("GetCartByUserId").WithTags("Cart");
        }
    }
}