using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Categories.Create
{
    public record CreateCategoryCommand(string Name, string? Description) : ICommand<int>;
}