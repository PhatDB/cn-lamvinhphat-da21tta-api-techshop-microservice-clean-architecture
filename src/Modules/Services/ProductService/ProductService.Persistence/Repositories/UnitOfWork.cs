using System.Data;
using System.Data.Common;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private DbTransaction? _currentTransaction;

        public UnitOfWork(ApplicationDbContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await _context.SaveChangesAsync(cancellationToken);

            List<Entity> domainEntities = _context.ChangeTracker.Entries<Entity>()
                .Where(e => e.Entity.DomainEvents?.Any() == true).Select(e => e.Entity).ToList();

            foreach (Entity entity in domainEntities)
            {
                List<INotification> events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (INotification domainEvent in events) await _mediator.Publish(domainEvent, cancellationToken);
            }

            return result;
        }

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("A transaction is already in progress.");

            DbConnection dbConnection = _context.Database.GetDbConnection();

            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();

            _currentTransaction = await dbConnection.BeginTransactionAsync();
            await _context.Database.UseTransactionAsync(_currentTransaction);

            return _currentTransaction;
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }

            await _context.DisposeAsync();
        }
    }
}