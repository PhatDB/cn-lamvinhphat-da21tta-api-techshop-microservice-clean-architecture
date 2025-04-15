using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Queries.Products.GetActiveProductByCategoryId;
using ProductService.Application.Queries.Products.GetActiveProductByName;
using ProductService.Application.Queries.Products.GetActiveProductDetail;
using ProductService.Application.Queries.Products.GetActiveProductFilter;
using ProductService.Application.Queries.Products.GetAllActiveProducts;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class ProductQueryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // Get All Products With PaginationOption
            app.MapGet("/products", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, ISender sender,
                CancellationToken cancellationToken) =>
            {
                PaginationOption paginationOption = new(sortBy, isDescending, pageNumber, pageSize);

                GetAllActiveProductQuery query = new(paginationOption);

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetAllProducts").Produces<PagedResult<GetAllProductDTO>>();

            // Get Products By Name
            app.MapGet("/products/{name}", async (
                string productName, int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, ISender sender,
                CancellationToken cancellationToken) =>
            {
                PaginationOption paginationOption = new(sortBy, isDescending, pageNumber, pageSize);
                GetProductByNameQuery query = new(productName, paginationOption);

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetProductsByName").Produces<PagedResult<GetAllProductDTO>>();

            // Get Products with filter
            app.MapGet("/products/filter", async (
                string? keyword, int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice, bool? isFeatured,
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, ISender sender,
                CancellationToken cancellationToken) =>
            {
                PaginationOption paginationOption = new(sortBy, isDescending, pageNumber, pageSize);

                GetActiveProductFilterQuery query = new(keyword, categoryId, brandId, minPrice, maxPrice, isFeatured,
                    paginationOption);

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(Results.Ok, CustomResults.Problem);
            }).WithTags("Product").WithName("FilterProducts").Produces<PagedResult<GetAllProductDTO>>();

            // Get Product By Id
            app.MapGet("products/{productId:int}",
                async (int productId, ISender sender, CancellationToken cancellationToken) =>
                {
                    GetActiveProductDetailQuery query = new(productId);
                    Result<ProductDetailDTO> result = await sender.Send(query, cancellationToken);

                    return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
                }).WithTags("Product").WithName("GetProductDetail");

            // Get Product By CategoryId
            app.MapGet("products/category/{categoryId:int}", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, int categoryId, ISender sender,
                CancellationToken cancellationToken) =>
            {
                PaginationOption paginationOption = new(sortBy, isDescending, pageNumber, pageSize);

                GetActiveProductByCategoryIdQuery query = new(categoryId, paginationOption);

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GeProductByCategoryId").Produces<PagedResult<GetAllProductDTO>>();
        }
    }
}