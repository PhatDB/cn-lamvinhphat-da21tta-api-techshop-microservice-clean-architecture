using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Products.DeleteImages
{
    public record DeleteImageCommand(int ProductId, List<int> ImageIds) : ICommand;
}