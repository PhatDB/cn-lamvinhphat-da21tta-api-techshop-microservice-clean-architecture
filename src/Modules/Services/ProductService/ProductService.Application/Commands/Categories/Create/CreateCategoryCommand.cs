using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Categories.Create
{
    public record CreateCategoryCommand(string Name, string? Description, string ImageContent) : ICommand<int>;
}