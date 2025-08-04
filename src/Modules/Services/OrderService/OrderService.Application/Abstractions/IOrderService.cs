using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;

namespace OrderService.Application.Abstractions
{
    public interface IOrderService
    {
        Task<Result<GetCartInfoResponse>> GetCartInfo(int cartId);

        Task<Result<GetListProductInfoResponse>> GetListProductInfo(List<int> productIds);

        Task<Result<CheckStockResponse>> CheckStockAsync(List<CheckStockRequest> items);

        Task<Result<GetCustomerInfoResponse>> GetCustomerInfo(int customerId);
    }
}