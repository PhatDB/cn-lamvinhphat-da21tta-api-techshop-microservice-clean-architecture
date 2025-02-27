using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.Create;
using ProductService.Application.DTOs;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("product", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new CreateProductCommand(
                    request.Name,
                    request.Sku,
                    request.Price,
                    request.CategoryId,
                    request.ProductImages ?? new List<ProductImageDTO>(),
                    request.Colors ?? new List<string>(),
                    request.Description,
                    request.DiscountPrice
                );

                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithTags("Product");
        }

        public sealed record Request(
            string Name,
            string Sku,
            decimal Price,
            int CategoryId,
            List<ProductImageDTO>? ProductImages,
            List<string>? Colors,
            string? Description = null,
            decimal? DiscountPrice = null
        );
    }
}