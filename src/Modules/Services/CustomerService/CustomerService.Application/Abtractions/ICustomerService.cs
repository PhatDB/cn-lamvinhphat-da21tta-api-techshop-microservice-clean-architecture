using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;

namespace CustomerService.Application.Abtractions
{
    public interface ICustomerService
    {
        Task<Result<HasCustomerPaidResponse>> HasCustomerPaid(int customerId, int productId);
        Task<Result<GetListProductInfoResponse>> ReviewGetListProductInfo(List<int> productIds);
    }
}