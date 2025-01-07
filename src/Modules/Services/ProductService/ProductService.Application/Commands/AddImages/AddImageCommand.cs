using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.AddImages
{
    public record AddImageCommand : ICommand
    {
        public AddImageCommand(int productId, List<ProductImageDTO>? productImages = null)
        {
            ProductId = productId;
            ProductImages = productImages ?? new List<ProductImageDTO>();
        }

        public int ProductId { get; init; }
        public List<ProductImageDTO> ProductImages { get; init; }
    }
}