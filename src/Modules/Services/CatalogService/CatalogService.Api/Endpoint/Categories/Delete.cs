using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CatalogService.Application.Commands.Delete;
using MediatR;

namespace CatalogService.Api.Endpoint.Categories
{
    public sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("category/{categoryId}", async (int categoryId, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteCategoryCommand query = new(categoryId);

                Result result = await sender.Send(query, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category");
        }
    }
}