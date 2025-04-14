using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.Create;
using ProductService.Application.Commands.Products.Delete;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class ProductCommandEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithName("CreateProduct").WithTags("Product");

            app.MapDelete("/products/{productId}",
                async (int productId, ISender sender, CancellationToken cancellationToken) =>
                {
                    DeleteProductCommand command = new(productId);

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }).WithTags("Product");
        }
    }
}