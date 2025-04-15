using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Brands.Delete
{
    public record DeleteBrandCommand(int BrandId) : ICommand;
}