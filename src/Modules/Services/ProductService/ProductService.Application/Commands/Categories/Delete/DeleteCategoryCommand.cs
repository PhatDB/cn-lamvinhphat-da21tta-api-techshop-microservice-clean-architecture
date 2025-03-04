using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Categories.Delete
{
    public record DeleteCategoryCommand(int Id) : ICommand;
}