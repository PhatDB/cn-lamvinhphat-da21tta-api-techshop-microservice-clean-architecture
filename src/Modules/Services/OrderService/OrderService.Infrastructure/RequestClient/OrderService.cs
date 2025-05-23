using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Products;
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
        private readonly IRequestClient<GetProductInfoRequest> _productClient;
        private readonly IRequestClient<IsCustomerExistRequest> _userClient;

        public OrderService(
            IRequestClient<GetCartInfo> cartClient, IRequestClient<IsCustomerExistRequest> userClient,
            IRequestClient<GetProductInfoRequest> productClient)
        {
            _cartClient = cartClient;
            _userClient = userClient;
            _productClient = productClient;
        }

        public async Task<Result<GetCartInfoResponse>> GetCartInfo(int cartId)
        {
            Response<GetCartInfoResponse> cartResponse =
                await _cartClient.GetResponse<GetCartInfoResponse>(new GetCartInfo(cartId));

            return Result.Success(cartResponse.Message);
        }

        public async Task<Result<GetListProductInfoRespone>> GetListProductInfo(List<int> productIds)
        {
            Response<GetListProductInfoRespone> productInfoResponse =
                await _productClient.GetResponse<GetListProductInfoRespone>(new GetListProductInfoRequest(productIds));
            return Result.Success(productInfoResponse.Message);
        }

        public async Task<Result> IsCustomerExist(int customerId)
        {
            Response<IsCustomerExistResponse> userResponse =
                await _userClient.GetResponse<IsCustomerExistResponse>(new IsCustomerExistRequest(customerId));
            if (!userResponse.Message.Exists)
                return Result.Failure(Error.NotFound("User.NotFound", "User is not found"));

            return Result.Success();
        }
    }
}