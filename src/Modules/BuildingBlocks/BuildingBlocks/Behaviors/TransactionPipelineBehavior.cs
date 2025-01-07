using System.Data;
using BuildingBlocks.Abstractions.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors
{
    public sealed class TransactionalPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionalPipelineBehavior(IUnitOfWork unitOfWork, ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting transaction for {RequestName}", typeof(TRequest).Name);

            IDbTransaction? transaction = null;

            try
            {
                transaction = await _unitOfWork.BeginTransactionAsync();
                if (transaction == null) throw new InvalidOperationException("Transaction could not be started. Check your IUnitOfWork implementation.");

                TResponse response = await next();

                transaction.Commit();
                _logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);

                return response;
            }
            catch (Exception ex)
            {
                if (transaction?.Connection != null)
                {
                    transaction.Rollback();
                    _logger.LogWarning(ex, "Transaction rolled back for {RequestName} due to an exception.", typeof(TRequest).Name);
                }

                _logger.LogError(ex, "An error occurred while handling {RequestName}", typeof(TRequest).Name);
                throw;
            }
            finally
            {
                transaction?.Dispose();
                _logger.LogInformation("Transaction disposed for {RequestName}", typeof(TRequest).Name);
            }
        }
    }
}