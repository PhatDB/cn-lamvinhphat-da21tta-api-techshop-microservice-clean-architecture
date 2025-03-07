﻿using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.Delete;

namespace ProductService.Api.Endpoint.Products.Commands
{
    public sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("product/{productId}", async (
                int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteProductCommand command = new(productId);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }
    }
}