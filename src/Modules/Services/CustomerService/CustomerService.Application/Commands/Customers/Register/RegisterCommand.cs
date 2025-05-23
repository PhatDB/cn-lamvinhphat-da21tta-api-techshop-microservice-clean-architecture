using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.Register
{
    public record RegisterCommand(string CustomerName, string Email, string Password) : ICommand<int>;
}