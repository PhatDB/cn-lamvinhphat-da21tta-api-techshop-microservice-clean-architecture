using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetAllProducts
{
    public record GetAllProductQuery : IQuery<PagedResult<GetAllProductDTO>>
    {
        public PaginationOption PaginationOption { get; set; } = new();
    }
}