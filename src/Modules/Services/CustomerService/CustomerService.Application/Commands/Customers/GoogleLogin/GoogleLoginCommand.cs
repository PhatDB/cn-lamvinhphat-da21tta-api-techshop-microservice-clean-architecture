using BuildingBlocks.CQRS;
using CustomerService.Application.DTOs;

namespace CustomerService.Application.Commands.Customers.GoogleLogin
{
    public record GoogleLoginCommand(string IdToken) : ICommand<LoginDto>;
}