using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AspNetCore.EventSourcing.Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;

namespace AspNetCore.EventSourcing.Infrastructure
{
    public sealed class BankingContext : DbContext
    {
        private static readonly ILoggerFactory DebugLoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
        private readonly IHostEnvironment? _env;

        public BankingContext(DbContextOptions<BankingContext> options,
            IHostEnvironment? env) : base(options)
        {
            _env = env;
        }

        public DbSet<AccountReadModel> Accounts { get; set; }
        public DbSet<TransactionReadModel> Transactions { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<EventStream> EventStreams { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_env != null && _env.IsDevelopment())
            {
                // used to print activity when debugging
                optionsBuilder.UseLoggerFactory(DebugLoggerFactory);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfiguration).Assembly);
            var aggregateTypes = modelBuilder.Model
                                             .GetEntityTypes()
                                             .Select(e => e.ClrType)
                                             .Where(e => !e.IsAbstract && e.IsAssignableTo(typeof(AggregateRoot)));

            foreach (var type in aggregateTypes)
            {
                var aggregateBuilder = modelBuilder.Entity(type);
                aggregateBuilder.Ignore(nameof(AggregateRoot.DomainEvents));
                aggregateBuilder.Property(nameof(AggregateRoot.Id)).ValueGeneratedNever();
            }

            var readModelTypes = modelBuilder.Model
                                             .GetEntityTypes()
                                             .Select(e => e.ClrType)
                                             .Where(e => !e.IsAbstract && e.IsAssignableTo(typeof(ReadModelBase)));

            foreach (var type in readModelTypes)
            {
                var readModelBuilder = modelBuilder.Entity(type);
                readModelBuilder.Property(nameof(ReadModelBase.Id)).ValueGeneratedNever();
            }
        }
    }
}
