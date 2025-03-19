using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using UserService.Application.DTOs;
using UserService.Application.Queries.Users.GetUserInfomation;

namespace UserService.Api.Endpoint.Users.Queries
{
    public sealed class GetUserInformation : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/{userId:int}", [Authorize] async (int userId, ISender sender) =>
            {
                Result<UserDTO> result = await sender.Send(new GetUserInfomationQuery(userId));

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("GetUserInformation").WithTags("User").RequireAuthorization();
        }
    }
}