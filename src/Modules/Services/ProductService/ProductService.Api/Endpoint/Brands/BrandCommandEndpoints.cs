using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Commands.Brands.Create;
using ProductService.Application.Commands.Brands.Delete;
using ProductService.Application.Commands.Brands.Update;

namespace ProductService.Api.Endpoint.Brands
{
    public class BrandCommandEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // Create Brand
            app.MapPost("/brands", async (
                CreateBrandCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                Result<int> result = await sender.Send(command, cancellationToken);

                return result.Match(success => Results.Ok(new { Id = result.Value }),
                    failure => CustomResults.Problem(failure));
            }).WithTags("Brand").WithName("CreateBrand");

            // Update Brand
            app.MapPut("/brands/{id:int}", async (
                int id, UpdateBrand request, ISender sender, CancellationToken cancellationToken) =>
            {
                UpdateBrandCommand command = new(id, request.BrandName, request.Description, request.IsActive);

                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Brand").WithName("UpdateBrand");

            // Delete Brand
            app.MapDelete("/brands/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                DeleteBrandCommand command = new(id);
                Result result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithTags("Brand").WithName("DeleteBrand");
        }

        private record UpdateBrand(string? BrandName, string? Description, bool? IsActive);
    }
}