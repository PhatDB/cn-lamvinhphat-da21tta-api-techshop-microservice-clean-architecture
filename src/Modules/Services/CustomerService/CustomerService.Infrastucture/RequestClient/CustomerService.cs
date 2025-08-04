using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using MassTransit;

namespace CustomerService.Infrastucture.RequestClient
{
    public class CustomerService : ICustomerService
    {
        private readonly IRequestClient<GetListProductInfoRequest> _productRequestClient;

        private readonly IRequestClient<HasCustomerPaidRequest> _requestClient;

        public CustomerService(
            IRequestClient<HasCustomerPaidRequest> requestClient,
            IRequestClient<GetListProductInfoRequest> productRequestClient)
        {
            _requestClient = requestClient;
            _productRequestClient = productRequestClient;
        }

        public async Task<Result<HasCustomerPaidResponse>> HasCustomerPaid(int customerId, int productId)
        {
            Response<HasCustomerPaidResponse> result =
                await _requestClient.GetResponse<HasCustomerPaidResponse>(
                    new HasCustomerPaidRequest(customerId, productId));

            return Result.Success(result.Message);
        }

        public async Task<Result<GetListProductInfoResponse>> ReviewGetListProductInfo(List<int> productIds)
        {
            Response<GetListProductInfoResponse> result =
                await _productRequestClient.GetResponse<GetListProductInfoResponse>(
                    new GetListProductInfoRequest(productIds));

            return Result.Success(result.Message);
        }
    }
}