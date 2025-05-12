using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Categories.Create;
using ProductService.Application.Commands.Categories.Delete;
using ProductService.Application.Commands.Categories.Update;

namespace ProductService.Api.Endpoint.Categories
{
    public sealed class CategoryCommandEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // Create Category
            app.MapPost("/categories", async (
                CreateCategoryCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }),
                    failure => CustomResults.Problem(failure));
            }).WithTags("Category").WithName("CreateCategory");

            // Delete Category
            app.MapDelete("/categories/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteCategoryCommand command = new(id);
                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category").WithName("DeleteCategory");

            // Update Category
            app.MapPut("/categories/{id:int}", async (
                int id, UpdateCategory request, ISender sender, CancellationToken cancellationToken) =>
            {
                UpdateCategoryCommand command = new(id, request.CategoryName, request.Description, request.ImageContent,
                    request.ParentId, request.IsActive);
                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category").WithName("UpdateCategory");
        }

        private record UpdateCategory(
            string? CategoryName,
            string? Description,
            string? ImageContent,
            int? ParentId,
            bool? IsActive);
    }
}