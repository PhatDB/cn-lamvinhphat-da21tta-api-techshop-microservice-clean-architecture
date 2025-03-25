using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Contracts.Users;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using MassTransit;

namespace CartService.Infrastructure.RequestClient
{
    public class CartService : ICartService
    {
        private readonly IRequestClient<GetProductInfo> _productClient;
        private readonly IRequestClient<UserExistRequest> _userClient;

        public CartService(IRequestClient<UserExistRequest> userClient, IRequestClient<GetProductInfo> productClient)
        {
            _userClient = userClient;
            _productClient = productClient;
        }

        public async Task<Result> IsUserExist(int userId)
        {
            Response<UserExistResponse> userResponse = await _userClient.GetResponse<UserExistResponse>(new UserExistRequest(userId));
            if (!userResponse.Message.Exists)
                return Result.Failure(Error.NotFound("User.NotFound", "User is not found"));

            return Result.Success();
        }

        public async Task<Result<ProductInfoResponse>> GetProductInfo(int productId)
        {
            Response<ProductInfoResponse> productResponse = await _productClient.GetResponse<ProductInfoResponse>(new GetProductInfo(productId));
            if (productResponse.Message == null)
                return Result.Failure<ProductInfoResponse>(Error.NotFound("Product.NotFound", "Product is not found"));

            return Result.Success(productResponse.Message);
        }
    }
}