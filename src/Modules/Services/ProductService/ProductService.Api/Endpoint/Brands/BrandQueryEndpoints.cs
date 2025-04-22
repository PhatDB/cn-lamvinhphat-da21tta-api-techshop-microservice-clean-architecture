using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using ProductService.Application.Queries.Brands.GetAllActiveBrands;
using ProductService.Domain.Entities;

namespace ProductService.Api.Endpoint.Brands
{
    public class BrandQueryEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // Get All Products With PaginationOption
            app.MapGet("/brands", async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetAllActiveBrandQuery query = new();
                Result<List<Brand>> result = await sender.Send(query, cancellationToken);
                return result.Match(success => Results.Ok(success), failure => CustomResults.Problem(failure));
            }).WithTags("Brand").WithName("GetAllActiveBrands");
        }
    }
}