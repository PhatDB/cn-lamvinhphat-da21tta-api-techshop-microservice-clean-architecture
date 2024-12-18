using BuildingBlocks.Results;
using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand : IRequest<Result>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

    public interface IBaseCommand;

}