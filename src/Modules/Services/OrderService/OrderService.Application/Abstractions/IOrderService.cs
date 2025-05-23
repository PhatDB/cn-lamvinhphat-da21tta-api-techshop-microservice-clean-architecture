using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;

namespace OrderService.Application.Abstractions
{
    public interface IOrderService
    {
        Task<Result<GetCartInfoResponse>> GetCartInfo(int cartId);

        Task<Result<GetListProductInfoRespone>> GetListProductInfo(List<int> productIds);
    }
}