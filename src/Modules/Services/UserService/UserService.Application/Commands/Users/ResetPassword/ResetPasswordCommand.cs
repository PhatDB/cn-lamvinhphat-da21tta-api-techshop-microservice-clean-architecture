using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.ResetPassword
{
    public record ResetPasswordCommand(string Email) : ICommand;
}