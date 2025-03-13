using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using MediatR;
using UserService.Application.Commands.Users.Login;
using UserService.Application.DTOs;

namespace UserService.Api.Endpoint.Users.Commands
{
    public sealed class Login : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/login", async (LoginCommand command, ISender sender) =>
            {
                Result<LoginDTO> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("LoginUser").WithTags("User");
        }
    }
}