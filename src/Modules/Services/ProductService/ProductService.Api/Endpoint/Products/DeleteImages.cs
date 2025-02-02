﻿using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands.DeleteImages;

namespace ProductService.Api.Endpoint.Products
{
    public class DeleteImages : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("product/{productId}/images", async (int productId, [FromBody] DeleteImageRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteImageCommand command = new(productId, request.ImageIds);
                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }

        public sealed record DeleteImageRequest(IEnumerable<int> ImageIds);
    }
}