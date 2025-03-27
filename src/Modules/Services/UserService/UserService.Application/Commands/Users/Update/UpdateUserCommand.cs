using BuildingBlocks.CQRS;

namespace UserService.Application.Commands.Users.Update
{
    public record UpdateUserCommand(int UserId, string? Username, string? Street, string? City, string? District, string? Ward, string? ZipCode, string? PhoneNumber) : ICommand;
}