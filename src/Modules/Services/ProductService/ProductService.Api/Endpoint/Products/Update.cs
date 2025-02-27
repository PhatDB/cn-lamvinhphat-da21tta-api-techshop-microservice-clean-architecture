using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.Update;

namespace ProductService.Api.Endpoint.Products
{
    public class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("product/{productId}", async (int productId, UpdateRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateProductCommand(
                    productId,
                    request.Name,
                    request.Sku,
                    request.Price,
                    request.CategoryId,
                    request.Description,
                    request.DiscountPrice
                );

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }
        
        public sealed record UpdateRequest(
            string Name,
            string Sku,
            decimal Price,
            int CategoryId,
            string? Description = null,
            decimal? DiscountPrice = null
        );
    }
}