using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Results;
using MediatR;
using UserService.Application.Commands.Users.Update;

namespace UserService.Api.Endpoint.Users.Commands
{
    public sealed class UpdateUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/user/update", async (UpdateUserCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
            }).WithName("UpdateUser").WithTags("User");
        }
    }
}