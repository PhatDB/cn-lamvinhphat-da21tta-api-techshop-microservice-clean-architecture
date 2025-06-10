using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Results;

namespace CustomerService.Application.Abtractions
{
    public interface ICustomerService
    {
        Task<Result<HasCustomerPaidResponse>> HasCustomerPaid(int customerId);
    }
}