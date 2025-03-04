using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Queries.Categories.GetAllCategory;
using ProductService.Domain.Entities;

namespace ProductService.Api.Endpoint.Categories.Queries
{
    public class GetAllCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("categories", async (
                ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllCategoryQuery query = new();
                Result<List<Category>> result =
                    await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success),
                    failure => CustomResults.Problem(failure));
            }).WithTags("Category").WithName("GetAllCategory");
        }
    }
}