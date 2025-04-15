using BuildingBlocks.CQRS;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetAllCategory
{
    public record GetAllCategoryQuery : IQuery<List<Category>>;
}