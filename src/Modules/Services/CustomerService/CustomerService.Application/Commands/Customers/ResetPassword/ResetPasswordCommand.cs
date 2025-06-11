using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.ResetPassword
{
    public record ResetPasswordCommand(string Email) : ICommand;
}