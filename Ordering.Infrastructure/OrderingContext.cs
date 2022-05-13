using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Ordering.Domain.Models;
using Ordering.Domain.Models.Aggregate;
using Ordering.Infrastructure.EntityConfigurations;
using System.Data;

namespace Ordering.Infrastructure;

public class OrderingContext : DbContext, IUnitOfWork {
    public const string DEFAULT_SCHEMA = "ordering";
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator) : base(options) {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemsEntityTypeConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync() {
        if (_currentTransaction != null) return null;
        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction) {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try {
            await SaveChangesAsync();
            transaction.Commit();
        } catch {
            RollbackTransaction();
            throw;
        } finally {
            if (_currentTransaction != null) {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction() {
        try {
            _currentTransaction?.Rollback();
        } finally {
            if (_currentTransaction != null) {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

public class OrderingContextDesignFactory : IDesignTimeDbContextFactory<OrderingContext> {
    public OrderingContext CreateDbContext(string[] args) {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new OrderingContext(optionsBuilder.Options, new NoMediator());
    }
}

public class NoMediator : IMediator {
    IAsyncEnumerable<TResponse> ISender.CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken) {
        return default(IAsyncEnumerable<TResponse>); ;
    }

    IAsyncEnumerable<object?> ISender.CreateStream(object request, CancellationToken cancellationToken) {
        return default(IAsyncEnumerable<object?>);
    }

    Task IPublisher.Publish(object notification, CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    Task IPublisher.Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    Task<TResponse> ISender.Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken) {
        return Task.FromResult<TResponse>(default(TResponse));
    }

    Task<object?> ISender.Send(object request, CancellationToken cancellationToken) {
        return Task.FromResult(default(object));
    }
}