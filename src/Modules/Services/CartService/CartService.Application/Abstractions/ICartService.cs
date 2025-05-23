using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;

namespace CartService.Application.Abstractions
{
    public interface ICartService
    {
        Task<Result> IsCustomerExist(int customerId);
        Task<Result<ProductInfoResponse>> GetProductInfo(int customerId);
    }
}