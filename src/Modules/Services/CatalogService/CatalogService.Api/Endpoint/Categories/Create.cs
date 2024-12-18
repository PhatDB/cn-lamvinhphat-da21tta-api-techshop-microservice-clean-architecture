using BuildingBlocks.Results;
using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure;
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
                    var command = new CreateCategoryCommand(
                        request.CategoryName,
                        request.Description,
                        request.ParentCategoryId,
                        request.CategoryItems);

                    Result<int> result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
                .WithTags("Category");
        }

        public sealed record Request(string CategoryName, string? Description, int? ParentCategoryId,List<CategoryItemDTO>? CategoryItems);
        
    }
}