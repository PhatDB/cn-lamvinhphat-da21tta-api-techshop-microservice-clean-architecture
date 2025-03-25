using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Queries.Users.GetUserInfomation;

namespace UserService.Api.Endpoint.Users.Queries
{
    public sealed class GetUserInformation : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/users/{userId:int}", async (int userId, ISender sender) =>
            {
                Result<UserDTO> result = await sender.Send(new GetUserInfomationQuery(userId));

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("GetUserInformation").WithTags("User");
        }
    }
}