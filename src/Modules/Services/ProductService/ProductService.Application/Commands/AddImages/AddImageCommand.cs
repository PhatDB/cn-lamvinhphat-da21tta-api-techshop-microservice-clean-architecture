using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.AddImages
{
    public record AddImageCommand(int ProductId, List<ProductImageDTO> ProductImages) : ICommand
    {
        public List<ProductImageDTO> ProductImages { get; init; } = ProductImages ?? new List<ProductImageDTO>();
    }
}