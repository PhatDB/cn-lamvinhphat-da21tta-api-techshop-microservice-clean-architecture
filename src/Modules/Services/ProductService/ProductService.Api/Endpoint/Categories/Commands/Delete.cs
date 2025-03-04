using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Categories.Delete;

namespace ProductService.Api.Endpoint.Categories.Commands
{
    public sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("category/{id}", async (
                int id, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteCategoryCommand command = new(id);
                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Category");
        }
    }
}