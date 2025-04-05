using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.VertifyOTP
{
    public record VertifyOTPCommand(string Email, string OTP) : ICommand;
}