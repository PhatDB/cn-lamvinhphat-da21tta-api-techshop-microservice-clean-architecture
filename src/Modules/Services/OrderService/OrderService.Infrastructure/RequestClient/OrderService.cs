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

        public async Task<Result<CheckStockResponse>> CheckStockAsync(List<CheckStockRequest> items)
        {
            if (items == null || items.Count == 0)
                return Result.Success(new CheckStockResponse(new List<CheckStockResult>()));

            List<int> productIds = items.Select(x => x.ProductId).Distinct().ToList();
            Response<GetListProductInfoResponse> response =
                await _productClient.GetResponse<GetListProductInfoResponse>(new GetListProductInfoRequest(productIds));

            List<ProductInfoResponse> products = response.Message.ProductInfos;

            List<CheckStockResult> checkResults = items.Select(item =>
            {
                ProductInfoResponse? p = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                int available = p?.Stock ?? 0;
                bool isOk = available >= item.QuantityRequested;

                return new CheckStockResult(item.ProductId, item.QuantityRequested, available, isOk);
            }).ToList();

            return Result.Success(new CheckStockResponse(checkResults));
        }

        public async Task<Result<GetCustomerInfoResponse>> GetCustomerInfo(int customerId)
        {
            Response<GetCustomerInfoResponse> response =
                await _customerClient.GetResponse<GetCustomerInfoResponse>(new GetCustomerInfoRequest(customerId));

            return Result.Success(response.Message);
        }
    }
}