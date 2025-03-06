using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.Products.GetProductByCategoryId
{
    public record GetProductByCategoryIdQuery(int CategoryId)
        : IQuery<PagedResult<GetAllProductDTO>>

    {
        public PaginationOption PaginationOption { get; set; } = new();
    }
}