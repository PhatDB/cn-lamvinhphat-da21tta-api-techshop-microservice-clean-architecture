using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;

namespace CartService.Application.Abstractions
{
    public interface ICartService
    {
        Task<Result> IsUserExist(int userId);
        Task<Result<ProductInfoResponse>> GetProductInfo(int userId);
    }
}