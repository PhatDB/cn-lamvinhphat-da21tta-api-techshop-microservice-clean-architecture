using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Queries.Categories.GetAllCategory;
using ProductService.Application.Queries.Categories.GetAllCategoryAdmin;
using ProductService.Application.Queries.Categories.GetCategoryById;
using ProductService.Domain.Entities;

namespace ProductService.Api.Endpoint.Categories
{
    public sealed class CategoryQueryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories", async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllCategoryQuery query = new();
                Result<List<Category>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Category").WithName("GetAllCategory");

            app.MapGet("/categories/admin", async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllCategoryAdminQuery query = new();
                Result<List<Category>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Category").WithName("GetAllCategoryAdmin");

            app.MapGet("/categories/{id:int}", async (int Id, ISender sender, CancellationToken cancellationToken) =>
            {
                GetCategoryByIdQuery query = new(Id);
                Result<Category> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Category").WithName("GetCategoryById");
        }
    }
}