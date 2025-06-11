using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Queries.Products.GetActiveProductByBrandId;
using ProductService.Application.Queries.Products.GetActiveProductByCategoryId;
using ProductService.Application.Queries.Products.GetActiveProductByName;
using ProductService.Application.Queries.Products.GetActiveProductDetail;
using ProductService.Application.Queries.Products.GetActiveProductFilter;
using ProductService.Application.Queries.Products.GetAllActiveProducts;
using ProductService.Application.Queries.Products.GetAllProducts;
using ProductService.Application.Queries.Products.GetTotalProducts;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class ProductQueryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, ISender sender,
                CancellationToken cancellationToken) =>
            {
                PaginationOption paginationOption = new(sortBy, isDescending, pageNumber, pageSize);

                GetAllActiveProductQuery query = new(paginationOption);

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetAllActiveProducts").Produces<PagedResult<GetAllProductDTO>>();

            // Get All Products
            app.MapGet("/products/admin", async (ISender sender, CancellationToken cancellationToken) =>
            {
                Result<List<GetAllProductDTO>> result = await sender.Send(new GetAllProductQuery(), cancellationToken);

                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetAllProduct").Produces<GetAllProductDTO>();

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
                }).WithTags("Product").WithName("GetProductDetail").Produces<ProductDetailDTO>();
            ;

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

            // Get Product By BrandId
            app.MapGet("products/brands/{brandId:int}", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, int brandId, ISender sender,
                CancellationToken cancellationToken) =>
            {
                PaginationOption paginationOption = new(sortBy, isDescending, pageNumber, pageSize);

                GetActiveProductByBrandIdQuery query = new(brandId, paginationOption);

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GeProductByBrandId").Produces<PagedResult<GetAllProductDTO>>();

            // Get Total Product
            app.MapGet("products/total", async (ISender sender) =>
            {
                GetTotalProductsQuery query = new();
                Result<TotalProductDto> result = await sender.Send(query);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetTotalProducts");
        }
    }
}