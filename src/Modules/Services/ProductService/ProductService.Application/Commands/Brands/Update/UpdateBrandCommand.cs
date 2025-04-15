using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Brands.Update
{
    public record UpdateBrandCommand(int BrandId, string? BrandName, string? Description, bool? IsActive) : ICommand;
}