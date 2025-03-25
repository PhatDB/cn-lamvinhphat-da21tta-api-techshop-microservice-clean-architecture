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
            app.MapPost("/user/change-password", async (ChangePasswordRequest request, ISender sender, ClaimsPrincipal user) =>
            {
                int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                ChangePasswordCommand command = new();
                command.UserId = userId;
                command.OldPassword = request.OldPassword;
                command.NewPassword = request.NewPassword;

                Result<int> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("ChangePasswordUser").WithTags("User");
        }

        public class ChangePasswordRequest
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}