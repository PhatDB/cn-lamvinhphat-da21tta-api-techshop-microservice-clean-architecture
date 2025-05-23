using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.ChangePassword
{
    public class ChangePasswordCommand : ICommand<int>
    {
        public int CustomerId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}