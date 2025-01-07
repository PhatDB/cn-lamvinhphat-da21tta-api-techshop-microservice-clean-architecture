using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Update;

namespace ProductService.Api.Endpoint.Products
{
    public class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("product/{productId}", async (int productId, UpdateRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                UpdateProductCommand command = new(productId, request.ProductName, request.Price, request.Description, request.DiscountPrice, request.SKU, request.Brand,
                    request.Model, request.StockStatus);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }

        public sealed record UpdateRequest(
            string? ProductName,
            decimal? Price,
            string? Description,
            decimal? DiscountPrice,
            string? SKU,
            string? Brand,
            string? Model,
            int? StockStatus);
    }
}