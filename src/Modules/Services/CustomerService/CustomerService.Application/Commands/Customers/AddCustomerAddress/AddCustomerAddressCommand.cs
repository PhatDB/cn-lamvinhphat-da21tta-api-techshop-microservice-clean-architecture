using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.AddCustomerAddress
{
    public record AddCustomerAddressCommand(
        int CustomerId,
        string Street,
        string Hamlet,
        string Ward,
        string District,
        string City) : ICommand<int>;
}