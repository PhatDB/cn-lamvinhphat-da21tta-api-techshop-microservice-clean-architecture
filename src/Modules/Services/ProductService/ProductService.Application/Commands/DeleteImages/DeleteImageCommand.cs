using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.DeleteImages
{
    public record DeleteImageCommand : ICommand
    {
        public DeleteImageCommand(int productId, IEnumerable<int> imageIds)
        {
            ProductId = productId;
            ImageIds = imageIds;
        }

        public int ProductId { get; init; }
        public IEnumerable<int> ImageIds { get; init; }
    }
}