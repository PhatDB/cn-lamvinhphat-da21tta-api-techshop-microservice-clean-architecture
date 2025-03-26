using System.Security.Claims;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using MediatR;
using UserService.Application.Commands.Users.ChangePassword;

namespace UserService.Api.Endpoint.Users.Commands
{
    public sealed class ChangePassword : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/user/change-password", async (ChangePasswordCommand command, ISender sender, ClaimsPrincipal user) =>
            {
                Result<int> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("ChangePasswordUser").WithTags("User");
        }
    }
}