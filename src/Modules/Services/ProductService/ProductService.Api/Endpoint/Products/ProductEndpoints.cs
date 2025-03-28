using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands.Products.AddImages;
using ProductService.Application.Commands.Products.Create;
using ProductService.Application.Commands.Products.Delete;
using ProductService.Application.Commands.Products.DeleteImages;
using ProductService.Application.Commands.Products.Update;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;
using ProductService.Application.Queries.Products.GetAllProducts;
using ProductService.Application.Queries.Products.GetProductByCategoryId;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class ProductEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/product/image/{productId}", async (int productId, AddImageRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                AddImageCommand command = new(productId, request.ProductImages);
                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");

            app.MapPost("/product", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                CreateProductCommand command = new(request.Name, request.Sku, request.Price, request.CategoryId, request.ProductImages, request.Inventory, request.Description,
                    request.DiscountPrice);

                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithTags("Product");

            app.MapDelete("/product/{productId}", async (int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteProductCommand command = new(productId);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");

            app.MapDelete("/product/{productId}/images", async (int productId, [FromBody] DeleteImageRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteImageCommand command = new(productId, request.ImageIds);
                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");

            app.MapPut("/product/{productId}", async (int productId, UpdateRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                UpdateProductCommand command = new(productId, request.Name, request.Sku, request.Price, request.CategoryId, request.SoldQuantity, request.IsActive,
                    request.Inventory, request.Description, request.DiscountPrice);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Product");

            app.MapGet("products", async (int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllProductQuery query = new()
                {
                    PaginationOption = new PaginationOption { PageNumber = pageNumber, PageSize = pageSize, SortBy = sortBy, IsDescending = isDescending }
                };

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetAllProducts").Produces<PagedResult<GetAllProductDTO>>();

            app.MapGet("products/category/{categoryId}", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, int categoryId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetProductByCategoryIdQuery query = new(categoryId)
                {
                    PaginationOption = new PaginationOption { PageNumber = pageNumber, PageSize = pageSize, SortBy = sortBy, IsDescending = isDescending }
                };

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GeProductByCategoryId").Produces<PagedResult<GetAllProductDTO>>();

            app.MapGet("product/{productId}", async (int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                GetProductDetailQuery query = new(productId);
                Result<ProductDetailDTO> result = await sender.Send(query, cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product");
        }


        public sealed record AddImageRequest(List<ProductImageDTO> ProductImages);

        public sealed record Request(
            string Name,
            string Sku,
            decimal Price,
            int CategoryId,
            List<ProductImageDTO> ProductImages,
            InventoryDTO Inventory,
            string? Description = null,
            decimal? DiscountPrice = null);

        public sealed record DeleteImageRequest(IEnumerable<int> ImageIds);

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