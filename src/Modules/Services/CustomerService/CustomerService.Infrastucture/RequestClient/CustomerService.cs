using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using MassTransit;

namespace CustomerService.Infrastucture.RequestClient
{
    public class CustomerService : ICustomerService
    {
        private readonly IRequestClient<HasCustomerPaidRequest> _requestClient;

        public CustomerService(IRequestClient<HasCustomerPaidRequest> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<Result<HasCustomerPaidResponse>> HasCustomerPaid(int customerId)
        {
            Response<HasCustomerPaidResponse> result =
                await _requestClient.GetResponse<HasCustomerPaidResponse>(new HasCustomerPaidRequest(customerId));

            return Result.Success(result.Message);
        }
    }
}