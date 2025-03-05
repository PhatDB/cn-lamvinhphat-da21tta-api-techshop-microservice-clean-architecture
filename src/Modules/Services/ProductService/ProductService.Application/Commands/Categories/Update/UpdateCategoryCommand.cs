using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Categories.Update
{
    public record UpdateCategoryCommand(
        int Id,
        string Name,
        string? Description,
        bool? IsActive) : ICommand;
}