/*using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Categories.Create;
using ProductService.Application.Commands.Categories.Delete;
using ProductService.Application.Commands.Categories.Update;
using ProductService.Application.Queries.Categories.GetAllCategory;
using ProductService.Domain.Entities;

namespace ProductService.Api.Endpoint.Categories
{
    public sealed class CategoryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/category", async (CreateCategoryRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                CreateCategoryCommand command = new(request.Name, request.Description);
                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithTags("Category");

            app.MapDelete("/category/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteCategoryCommand command = new(id);
                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category");

            app.MapPut("/category/{id}", async (int id, UpadateCategoryRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                UpdateCategoryCommand command = new(id, request.Name, request.Description, request.IsActive);
                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category");

            app.MapGet("/categories", async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllCategoryQuery query = new();
                Result<List<Category>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Category").WithName("GetAllCategory");
        }

        public sealed record CreateCategoryRequest(string Name, string? Description = null);

        public sealed record UpadateCategoryRequest(string? Name, bool? IsActive, string? Description = null);
    }
}*/

