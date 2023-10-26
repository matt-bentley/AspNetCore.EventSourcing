using AspNetCore.EventSourcing.Infrastructure;
using AspNetCore.EventSourcing.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.EventSourcing.Migrations.Factories
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<BankingContext> Create(IConfiguration configuration)
        {
            var appSettings = DatabaseSettings.Create(configuration);

            return new DbContextOptionsBuilder<BankingContext>()
                .UseSqlServer(appSettings.SqlConnectionString, b => b.MigrationsAssembly("AspNetCore.EventSourcing.Migrations"))
                .Options;
        }
    }
}
