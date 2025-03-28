using System.Security.Claims;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using UserService.Application.Commands.Users.ChangePassword;
using UserService.Application.Commands.Users.Login;
using UserService.Application.Commands.Users.Logout;
using UserService.Application.Commands.Users.Register;
using UserService.Application.Commands.Users.Update;
using UserService.Application.DTOs;
using UserService.Application.Queries.Users.GetUserInfomation;

namespace UserService.Api.Endpoint.Users
{
    public sealed class UserEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/user/change-password", async (ChangePasswordCommand command, ISender sender, ClaimsPrincipal user) =>
            {
                Result<int> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("ChangePasswordUser").WithTags("User");

            app.MapPost("/user/login", async (LoginCommand command, ISender sender) =>
            {
                Result<LoginDTO> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("LoginUser").WithTags("User");

            app.MapPost("/user/logout", async (LogoutCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("LogoutUser").WithTags("User");

            app.MapPost("/user/register", async (RegisterCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);

                return result.Match(success => Results.Ok(new { Id = result.Value }), failure => CustomResults.Problem(failure));
            }).WithName("RegisterUser").WithTags("User");

            app.MapPut("/user/update", async (UpdateUserCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
            }).WithName("UpdateUser").WithTags("User");

            app.MapGet("/users/{userId:int}", async (int userId, ISender sender) =>
            {
                Result<UserDTO> result = await sender.Send(new GetUserInfomationQuery(userId));

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).WithName("GetUserInformation").WithTags("User");
        }
    }
}