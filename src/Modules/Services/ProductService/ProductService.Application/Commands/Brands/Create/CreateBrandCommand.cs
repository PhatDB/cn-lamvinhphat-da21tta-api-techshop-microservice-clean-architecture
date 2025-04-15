using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Brands.Create
{
    public record CreateBrandCommand(string BrandName, string Description) : ICommand<int>;
}