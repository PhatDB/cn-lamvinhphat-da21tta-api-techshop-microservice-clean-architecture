using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.ChangePassword
{
    public class ChangePasswordCommand : ICommand<int>
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}