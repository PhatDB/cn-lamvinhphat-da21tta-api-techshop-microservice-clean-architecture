using BuildingBlocks.Results;
using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}