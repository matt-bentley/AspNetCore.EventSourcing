using Autofac;
using AspNetCore.EventSourcing.Application.AutofacModules;
using AspNetCore.EventSourcing.Hosting;
using AspNetCore.EventSourcing.Infrastructure.AutofacModules;
using AspNetCore.EventSourcing.Migrations;
using AspNetCore.EventSourcing.Migrations.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        var hostBuilder = Worker.CreateBuilder(args)
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddHostedService<MigrationJob>();
                        })
                        .ConfigureContainer<ContainerBuilder>((hostContext, container) =>
                        {
                            container.RegisterModule(new InfrastructureModule(DbContextOptionsFactory.Create(hostContext.Configuration), hostContext.Configuration));
                            container.RegisterModule(new ApplicationModule());
                        });

        await hostBuilder.BuildAndRunAsync();
    }

    // EF Core uses this method at design time to access the DbContext
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args);
}

