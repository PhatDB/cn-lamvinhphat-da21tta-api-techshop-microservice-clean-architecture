using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using MediatR;
using UserService.Application.Commands.Users.Register;

namespace UserService.Api.Endpoint.Users.Commands
{
    public sealed class Register : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/users/register",
                async (RegisterCommand command, ISender sender) =>
                {
                    Result<int> result = await sender.Send(command);

                    return result.Match(success => Results.Ok(new { Id = result.Value }),
                        failure => CustomResults.Problem(failure));
                }).WithName("RegisterUser").WithTags("User");
            // .Produces<int>(StatusCodes.Status201Created)
            // .Produces<string>(StatusCodes.Status400BadRequest)
            // .WithSummary("User Registration").WithDescription(
            //     "Creates a new user account with username, email, and password.");
        }
    }
}