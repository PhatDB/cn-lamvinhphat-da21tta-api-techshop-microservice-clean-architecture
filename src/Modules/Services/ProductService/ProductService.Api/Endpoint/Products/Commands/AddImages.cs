using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.AddImages;
using ProductService.Application.DTOs;

namespace ProductService.Api.Endpoint.Products.Commands
{
    public sealed class AddImages : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("product/image/{productId}", async (
                int productId, AddImageRequest request, ISender sender,
                CancellationToken cancellationToken) =>
            {
                AddImageCommand command = new(productId, request.ProductImages);
                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }

        public sealed record AddImageRequest(List<ProductImageDTO> ProductImages);
    }
}