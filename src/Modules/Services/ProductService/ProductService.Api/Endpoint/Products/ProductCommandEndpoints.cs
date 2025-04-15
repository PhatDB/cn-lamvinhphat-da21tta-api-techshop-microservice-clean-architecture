using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Products.Create;
using ProductService.Application.Commands.Products.Delete;
using ProductService.Application.Commands.Products.Update;
using ProductService.Application.DTOs;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class ProductCommandEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // Create Product
            app.MapPost("/products", async (CreateProductCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithName("CreateProduct").WithTags("Product");

            // Update Product
            app.MapPut("/products/{productId:int}",
                async (int productId, UpdateProductRequest request, ISender sender) =>
                {
                    UpdateProductCommand command = new(productId, request.ProductName, request.Price, request.Stock,
                        request.CategoryId, request.BrandId, request.Discount, request.SoldQuantity, request.IsActive,
                        request.IsFeatured, request.Description, request.Specs, request.NewImages,
                        request.ImageIdsToRemove);

                    Result result = await sender.Send(command);
                    return result.Match(Results.NoContent, CustomResults.Problem);
                }).WithName("UpdateProduct").WithTags("Product");

            // Delete Product
            app.MapDelete("/products/{productId}",
                async (int productId, ISender sender, CancellationToken cancellationToken) =>
                {
                    DeleteProductCommand command = new(productId);
                    Result result = await sender.Send(command, cancellationToken);
                    return result.Match(Results.NoContent, CustomResults.Problem);
                }).WithName("DeleteProduct").WithTags("Product");
        }

        private record UpdateProductRequest(
            string? ProductName,
            decimal? Price,
            int? Stock,
            int? CategoryId,
            int? BrandId,
            decimal? Discount,
            int? SoldQuantity,
            bool? IsActive,
            bool? IsFeatured,
            string? Description,
            string? Specs,
            List<ProductImageDto>? NewImages = null,
            List<int>? ImageIdsToRemove = null);
    }
}