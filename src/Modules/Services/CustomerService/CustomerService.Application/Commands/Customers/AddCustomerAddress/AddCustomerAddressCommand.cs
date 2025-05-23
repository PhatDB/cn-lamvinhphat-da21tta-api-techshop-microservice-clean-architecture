using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.AddCustomerAddress
{
    public record AddCustomerAddressCommand(
        int CustomerId,
        string Street,
        string Hemlet,
        string Ward,
        string District,
        string City) : ICommand<int>;
}