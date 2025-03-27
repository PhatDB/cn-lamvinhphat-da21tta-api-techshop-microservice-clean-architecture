using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Results;

namespace OrderService.Application.Abstractions
{
    public interface IOrderService
    {
        Task<Result<GetCartInfoResponse>> GetCartInfo(int userId);
        Task<Result> IsUserExist(int userId);
    }
}