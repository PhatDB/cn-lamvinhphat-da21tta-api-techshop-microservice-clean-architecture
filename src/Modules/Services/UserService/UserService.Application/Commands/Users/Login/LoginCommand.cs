using BuildingBlocks.CQRS;
using UserService.Application.DTOs;

namespace UserService.Application.Commands.Users.Login
{
    public class LoginCommand : ICommand<LoginDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}