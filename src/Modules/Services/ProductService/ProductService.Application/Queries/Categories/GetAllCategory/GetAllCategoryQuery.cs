using BuildingBlocks.CQRS;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetAllCategory
{
    public class GetAllCategoryQuery : IQuery<List<Category>>
    {
    }
}