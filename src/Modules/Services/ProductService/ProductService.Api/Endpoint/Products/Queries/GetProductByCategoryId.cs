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
            app.MapGet("products/category/{categoryId}", async (
                    int? pageNumber, int? pageSize, string? sortBy, bool? isDescending,
                    int categoryId, ISender sender,
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

                    Result<PagedResult<GetAllProductDTO>> result =
                        await sender.Send(query, cancellationToken);
                    return result.Match(success => Results.Ok(success),
                        failure => CustomResults.Problem(failure));
                }).WithTags("Product").WithName("GeProductByCategoryId")
                .Produces<PagedResult<GetAllProductDTO>>();
        }
    }
}