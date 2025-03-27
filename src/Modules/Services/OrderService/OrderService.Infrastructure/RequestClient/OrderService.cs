using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Users;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using MassTransit;
using OrderService.Application.Abstractions;

namespace OrderService.Infrastructure.RequestClient
{
    public class OrderService : IOrderService
    {
        private readonly IRequestClient<GetCartInfo> _cartClient;
        private readonly IRequestClient<UserExistRequest> _userClient;

        public OrderService(IRequestClient<GetCartInfo> cartClient, IRequestClient<UserExistRequest> userClient)
        {
            _cartClient = cartClient;
            _userClient = userClient;
        }


        public async Task<Result<GetCartInfoResponse>> GetCartInfo(int userId)
        {
            Response<GetCartInfoResponse> cartResponse = await _cartClient.GetResponse<GetCartInfoResponse>(new GetCartInfo(userId));
            if (cartResponse.Message.UserId == 0)
                return Result.Failure<GetCartInfoResponse>(Error.NotFound("Product.NotFound", "Product is not found"));

            return Result.Success(cartResponse.Message);
        }

        public async Task<Result> IsUserExist(int userId)
        {
            Response<UserExistResponse> userResponse = await _userClient.GetResponse<UserExistResponse>(new UserExistRequest(userId));
            if (!userResponse.Message.Exists)
                return Result.Failure(Error.NotFound("User.NotFound", "User is not found"));

            return Result.Success();
        }
    }
}