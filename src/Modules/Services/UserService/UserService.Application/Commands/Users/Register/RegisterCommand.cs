using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.Register
{
    public record RegisterCommand(string Username, string Email, string Password)
        : ICommand<int>;
}