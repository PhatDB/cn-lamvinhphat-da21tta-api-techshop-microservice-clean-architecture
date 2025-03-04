using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Categories.Create;

namespace ProductService.Api.Endpoint.Categories.Commands
{
    public sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("category", async (
                CreateCategoryRequest request, ISender sender,
                CancellationToken cancellationToken) =>
            {
                CreateCategoryCommand command = new(request.Name, request.Description);
                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }),
                    failure => CustomResults.Problem(failure));
            }).WithTags("Category");
        }

        public sealed record CreateCategoryRequest(
            string Name,
            string? Description = null);
    }
}