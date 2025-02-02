﻿using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Queries;
using ProductService.Domain.Entities;

namespace ProductService.Api.Endpoint.Products
{
    public class GetProductDetail : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("product/{productId}", async (int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetProductDetailQuery query = new(productId);
                Result<Product> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product");
        }
    }
}