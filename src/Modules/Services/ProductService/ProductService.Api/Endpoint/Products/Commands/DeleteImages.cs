using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands.Products.DeleteImages;

namespace ProductService.Api.Endpoint.Products.Commands
{
    public sealed class DeleteImages : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/product/{productId}/images", async (int productId, [FromBody] DeleteImageRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteImageCommand command = new(productId, request.ImageIds);
                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }

        public sealed record DeleteImageRequest(IEnumerable<int> ImageIds);
    }
}