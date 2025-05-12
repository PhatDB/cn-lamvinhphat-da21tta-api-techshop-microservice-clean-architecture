using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Categories.Update
{
    public record UpdateCategoryCommand(
        int Id,
        string? CategoryName,
        string? Description,
        string? ImageContent,
        int? ParentId,
        bool? IsActive) : ICommand;
}