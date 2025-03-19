using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.Update
{
    public record UpdateUserCommand(int UserId, string? Username, string? AddressLine, string? PhoneNumber, string? Province, string? District) : ICommand;
}