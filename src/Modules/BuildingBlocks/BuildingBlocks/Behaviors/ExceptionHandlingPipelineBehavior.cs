using System.Reflection;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors
{
    public class ExceptionHandlingPipelineBehavior<TRequest, TResponse>(
        ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in {Request}", typeof(TRequest).Name);

                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    Type resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
                    MethodInfo? failureMethod = resultType.GetMethod("Failure", [typeof(string)]);
                    return (TResponse)failureMethod.Invoke(null, [ex.Message]);
                }

                throw;
            }
        }
    }
}