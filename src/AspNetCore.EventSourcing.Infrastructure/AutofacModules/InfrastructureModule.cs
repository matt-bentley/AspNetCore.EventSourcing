using Autofac;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Infrastructure.Repositories;
using AspNetCore.EventSourcing.Infrastructure.Services;
using AspNetCore.EventSourcing.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AspNetCore.EventSourcing.Infrastructure.Repositories.EventStore;

namespace AspNetCore.EventSourcing.Infrastructure.AutofacModules
{
    public sealed class InfrastructureModule : Module
    {
        private readonly DbContextOptions<BankingContext> _options;
        private readonly IConfiguration Configuration;

        public InfrastructureModule(IConfiguration configuration) : this(CreateDbOptions(configuration), configuration)
        {

        }

        public InfrastructureModule(DbContextOptions<BankingContext> options, IConfiguration configuration)
        {
            Configuration = configuration;
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(Options.Create(DatabaseSettings.Create(Configuration)));
            builder.RegisterType<BankingContext>()
                .AsSelf()
                .InstancePerRequest()
                .InstancePerLifetimeScope()
                .WithParameter(new NamedParameter("options", _options));

            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>));

            builder.RegisterGeneric(typeof(EventStreamRepository<>))
                .As(typeof(IEventStreamRepository<>))
                .InstancePerRequest()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReadModelRepository<>))
                .As(typeof(IReadModelRepository<>));

            builder.RegisterType<EntityFrameworkEventStore>()
                .AsImplementedInterfaces();

            builder.RegisterType<NotificationsService>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static DbContextOptions<BankingContext> CreateDbOptions(IConfiguration configuration)
        {
            var databaseSettings = DatabaseSettings.Create(configuration);
            var builder = new DbContextOptionsBuilder<BankingContext>();
            builder.UseSqlServer(databaseSettings.SqlConnectionString);
            return builder.Options;
        }
    }
}
