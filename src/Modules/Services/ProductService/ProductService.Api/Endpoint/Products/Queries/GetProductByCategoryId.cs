using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Queries.Products.GetProductByCategoryId;

namespace ProductService.Api.Endpoint.Products.Queries
{
    public sealed class GetProductByCategoryId : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("product/category/{categoryId}", async (
                int categoryId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetProductByCategoryIdQuery query =
                    new GetProductByCategoryIdQuery(categoryId);
                Result<List<GetAllProductDTO>> result =
                    await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            }).WithName("GetProductsByCategoryId").WithTags("Product");
        }
    }
}