using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Create;
using ProductService.Application.DTOs;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("product", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                CreateProductCommand command = new(request.ProductName, request.Price, request.Description, request.DiscountPrice, request.SKU, request.Brand, request.Model,
                    request.StockStatus, request.ProductImages);

                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithTags("Product");
        }

        public sealed record Request(
            string ProductName,
            decimal Price,
            string? Description,
            decimal? DiscountPrice,
            string? SKU,
            string? Brand,
            string? Model,
            int? StockStatus,
            List<ProductImageDTO>? ProductImages);
    }
}