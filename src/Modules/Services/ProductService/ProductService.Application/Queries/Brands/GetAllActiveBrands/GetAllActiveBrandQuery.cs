using BuildingBlocks.CQRS;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Brands.GetAllActiveBrands
{
    public record GetAllActiveBrandQuery : IQuery<List<Brand>>;
}