using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.Update;
using ProductService.Application.DTOs;

namespace ProductService.Api.Endpoint.Products.Commands
{
    public sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("product/{productId}", async (
                int productId, UpdateRequest request, ISender sender,
                CancellationToken cancellationToken) =>
            {
                UpdateProductCommand command = new(productId, request.Name, request.Sku,
                    request.Price, request.CategoryId, request.SoldQuantity,
                    request.IsActive, request.Inventory, request.Description,
                    request.DiscountPrice);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");
        }

        public sealed record UpdateRequest(
            string? Name,
            string? Sku,
            decimal? Price,
            int? CategoryId,
            int? SoldQuantity,
            bool? IsActive,
            InventoryDTO Inventory,
            string? Description,
            decimal? DiscountPrice);
    }
}