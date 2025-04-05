using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.SendOTP
{
    public record SendOTPCommand(string Email) : ICommand;
}