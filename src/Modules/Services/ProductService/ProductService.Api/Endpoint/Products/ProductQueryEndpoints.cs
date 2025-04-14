using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;
using ProductService.Application.Queries.Products.GetAllProducts;
using ProductService.Application.Queries.Products.GetProductByCategoryId;

namespace ProductService.Api.Endpoint.Products
{
    public sealed class ProductQueryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, ISender sender,
                CancellationToken cancellationToken) =>
            {
                GetAllProductQuery query = new()
                {
                    PaginationOption = new PaginationOption
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        SortBy = sortBy,
                        IsDescending = isDescending
                    }
                };

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GetAllProducts").Produces<PagedResult<GetAllProductDTO>>();

            app.MapGet("products/category/{categoryId}", async (
                int? pageNumber, int? pageSize, string? sortBy, bool? isDescending, int categoryId, ISender sender,
                CancellationToken cancellationToken) =>
            {
                GetProductByCategoryIdQuery query = new(categoryId)
                {
                    PaginationOption = new PaginationOption
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        SortBy = sortBy,
                        IsDescending = isDescending
                    }
                };

                Result<PagedResult<GetAllProductDTO>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Product").WithName("GeProductByCategoryId").Produces<PagedResult<GetAllProductDTO>>();

            app.MapGet("product/{productId}",
                async (int productId, ISender sender, CancellationToken cancellationToken) =>
                {
                    GetProductDetailQuery query = new(productId);
                    Result<ProductDetailDTO> result = await sender.Send(query, cancellationToken);

                    return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
                }).WithTags("Product");
        }
    }
}