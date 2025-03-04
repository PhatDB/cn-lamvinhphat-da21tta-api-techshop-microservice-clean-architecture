using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Queries.Products.GetAllProducts;

namespace ProductService.Api.Endpoint.Products.Queries
{
    public class GetAllProducts : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products", async (
                    int? pageNumber, int? pageSize, string? sortBy, bool? isDescending,
                    ISender sender, CancellationToken cancellationToken) =>
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

                    Result<PagedResult<GetAllProductDTO>> result =
                        await sender.Send(query, cancellationToken);
                    return result.Match(success => Results.Ok(success),
                        failure => CustomResults.Problem(failure));
                }).WithTags("Product").WithName("GetAllProducts")
                .Produces<PagedResult<GetAllProductDTO>>();
        }
    }
}