using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Categories.Update;

namespace ProductService.Api.Endpoint.Categories.Commands
{
    public sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/category/{id}", async (int id, UpadateCategoryRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                UpdateCategoryCommand command = new(id, request.Name, request.Description, request.IsActive);
                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category");
        }

        public sealed record UpadateCategoryRequest(string? Name, bool? IsActive, string? Description = null);
    }
}