using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CatalogService.Application.Commands.Create;
using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Api.Endpoint.Categories
{
    public sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("category", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                CreateCategoryCommand command = new(request.CategoryName, request.Description, request.ParentCategoryId, request.CategoryItems);

                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithTags("Category");
        }

        public sealed record Request(string CategoryName, string? Description, int? ParentCategoryId, List<CategoryItemDTO>? CategoryItems);
    }
}