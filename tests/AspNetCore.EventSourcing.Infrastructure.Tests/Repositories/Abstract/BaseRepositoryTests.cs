using MediatR;
using Autofac;
using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Infrastructure.AutofacModules;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AspNetCore.EventSourcing.Application.AutofacModules;
using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using AspNetCore.EventSourcing.Core.Customers.Entities;

namespace AspNetCore.EventSourcing.Infrastructure.Tests.Repositories.Abstract
{
    public abstract class BaseRepositoryTests : IAsyncLifetime
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly BankingContext Database;
        private readonly IContainer _container;

        public BaseRepositoryTests() : this(true)
        {

        }

        public BaseRepositoryTests(bool mockMediator)
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<BankingContext>()
                    .UseSqlite(_connection)
                    .Options;

            var services = new ServiceCollection()
            .AddLogging();

            var configuration = new ConfigurationBuilder().Build();
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            var env = Mock.Of<IHostEnvironment>();
            containerBuilder.RegisterInstance(env);

            var mediator = Mock.Of<IMediator>();
            if (mockMediator)
            {
                containerBuilder.RegisterInstance(mediator);
            }
            else
            {
                containerBuilder.RegisterModule(new ApplicationModule());
            }

            Database = new BankingContext(options, env);
            Database.Database.EnsureCreated();

            containerBuilder.RegisterModule(new InfrastructureModule(options, configuration));
            _container = containerBuilder.Build();
        }

        public async Task InitializeAsync()
        {
            var customersRepository = GetRepository<Customer>();
            await GetUnitOfWork().CommitAsync();
        }

        public Task DisposeAsync()
        {
            Database.Dispose();
            _connection.Close();
            _connection.Dispose();
            return Task.CompletedTask;
        }

        protected IRepository<T> GetRepository<T>()
        where T : AggregateRoot
        {
            return _container.Resolve<IRepository<T>>();
        }

        protected IEventStreamRepository<T> GetEventStreamRepository<T>() where T : EventSourcedAggregate
        {
            return _container.Resolve<IEventStreamRepository<T>>();
        }

        protected IReadModelRepository<T> GetReadModelRepository<T>() where T : ReadModelBase
        {
            return _container.Resolve<IReadModelRepository<T>>();
        }

        protected IUnitOfWork GetUnitOfWork()
        {
            return _container.Resolve<IUnitOfWork>();
        }
    }
}
