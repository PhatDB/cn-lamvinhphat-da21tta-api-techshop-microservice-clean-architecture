using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using UserService.Application.Commands.Users.Logout;

namespace UserService.Api.Endpoint.Users.Commands
{
    public sealed class Logout : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/logout", [Authorize] async (LogoutCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("LogoutUser").WithTags("User").RequireAuthorization();
        }
    }
}