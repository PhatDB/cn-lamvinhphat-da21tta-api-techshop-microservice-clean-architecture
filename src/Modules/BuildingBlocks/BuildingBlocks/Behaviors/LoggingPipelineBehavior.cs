using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            Stopwatch stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("➡️ Handling {RequestName} with data: {@Request}", requestName, request);

            TResponse response = await next();

            stopwatch.Stop();

            _logger.LogInformation("✅ Handled {RequestName} in {ElapsedMilliseconds}ms", requestName,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
    }
}