using BuildingBlocks.CQRS;
using CustomerService.Application.DTOs;

namespace CustomerService.Application.Commands.Customers.Login
{
    public class LoginCommand : ICommand<LoginDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}