using BuildingBlocks.CQRS;
using UserService.Application.DTOs;

namespace UserService.Application.Queries.Users.GetUserInfomation
{
    public record GetUserInfomationQuery(int UserId) : IQuery<UserDTO>;
}