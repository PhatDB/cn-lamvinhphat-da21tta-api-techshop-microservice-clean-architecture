using BuildingBlocks.CQRS;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IQuery<Category>;
}