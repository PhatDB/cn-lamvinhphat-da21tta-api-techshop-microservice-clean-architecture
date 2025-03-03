using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;

namespace ProductService.Api.Endpoint.Products.Queries
{
    public class GetProductDetail : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("product/{productId}", async (
                int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetProductDetailQuery query = new(productId);
                Result<ProductDetailDTO> result =
                    await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success),
                    failure => CustomResults.Problem(failure));
            }).WithTags("Product");
        }
    }
}