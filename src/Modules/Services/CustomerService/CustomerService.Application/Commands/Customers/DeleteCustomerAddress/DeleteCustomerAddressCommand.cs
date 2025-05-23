using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.DeleteCustomerAddress
{
    public record DeleteCustomerAddressCommand(int AddressId) : ICommand;
}