using BuildingBlocks.CQRS;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetAllCategoryAdmin
{
    public record GetAllCategoryAdminQuery : IQuery<List<Category>>;
}