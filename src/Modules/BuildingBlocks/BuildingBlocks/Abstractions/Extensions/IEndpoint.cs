using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Abstractions.Extensions
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }
}