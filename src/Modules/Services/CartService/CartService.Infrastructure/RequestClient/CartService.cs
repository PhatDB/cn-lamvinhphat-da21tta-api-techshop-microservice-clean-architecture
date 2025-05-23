using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Contracts.Users;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using MassTransit;

namespace CartService.Infrastructure.RequestClient
{
    public class CartService : ICartService
    {
        private readonly IRequestClient<GetProductInfoRequest> _productClient;
        private readonly IRequestClient<IsCustomerExistRequest> _userClient;

        public CartService(
            IRequestClient<IsCustomerExistRequest> userClient, IRequestClient<GetProductInfoRequest> productClient)
        {
            _userClient = userClient;
            _productClient = productClient;
        }

        public async Task<Result<ProductInfoResponse>> GetProductInfo(int productId)
        {
            Response<ProductInfoResponse> productResponse =
                await _productClient.GetResponse<ProductInfoResponse>(new GetProductInfoRequest(productId));

            return Result.Success(productResponse.Message);
        }

        public async Task<Result> IsCustomerExist(int customerId)
        {
            Response<IsCustomerExistResponse> userResponse =
                await _userClient.GetResponse<IsCustomerExistResponse>(new IsCustomerExistRequest(customerId));

            return Result.Success();
        }
    }
}