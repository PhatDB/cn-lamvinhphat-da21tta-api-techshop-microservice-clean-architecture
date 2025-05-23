using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.VerifyOTP
{
    public record VerifyOTPCommand(string Email, string Otp) : ICommand;
}