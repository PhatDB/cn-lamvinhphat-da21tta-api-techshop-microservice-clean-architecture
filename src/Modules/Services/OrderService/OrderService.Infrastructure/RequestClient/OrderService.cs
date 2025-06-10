using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;
using MassTransit;
using OrderService.Application.Abstractions;

namespace OrderService.Infrastructure.RequestClient
{
    public class OrderService : IOrderService
    {
        private readonly IRequestClient<GetCartInfo> _cartClient;
        private readonly IRequestClient<GetCustomerInfoRequest> _customerClient;
        private readonly IRequestClient<GetListProductInfoRequest> _productClient;

        public OrderService(
            IRequestClient<GetCartInfo> cartClient, IRequestClient<GetListProductInfoRequest> productClient,
            IRequestClient<GetCustomerInfoRequest> customerClient)
        {
            _cartClient = cartClient;
            _productClient = productClient;
            _customerClient = customerClient;
        }


        public async Task<Result<GetCartInfoResponse>> GetCartInfo(int cartId)
        {
            Response<GetCartInfoResponse> cartResponse =
                await _cartClient.GetResponse<GetCartInfoResponse>(new GetCartInfo(cartId));

            return Result.Success(cartResponse.Message);
        }

        public async Task<Result<GetListProductInfoResponse>> GetListProductInfo(List<int> productIds)
        {
            Response<GetListProductInfoResponse> productInfoResponse =
                await _productClient.GetResponse<GetListProductInfoResponse>(new GetListProductInfoRequest(productIds));
            return Result.Success(productInfoResponse.Message);
        }

        public async Task<Result<GetCustomerInfoResponse>> GetCustomerInfo(int customerId)
        {
            Response<GetCustomerInfoResponse> customerInfoResponse =
                await _customerClient.GetResponse<GetCustomerInfoResponse>(new GetCustomerInfoRequest(customerId));
            return Result.Success(customerInfoResponse.Message);
        }
    }
}