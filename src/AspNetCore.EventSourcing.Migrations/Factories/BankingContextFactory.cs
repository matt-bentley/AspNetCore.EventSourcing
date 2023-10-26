using AspNetCore.EventSourcing.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.EventSourcing.Migrations.Factories
{
    public class BankingContextFactory : IDesignTimeDbContextFactory<BankingContext>
    {
        private readonly IConfiguration _configuration;

        public BankingContextFactory()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json")
                   .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public BankingContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BankingContext CreateDbContext(string[] args)
        {
            return new BankingContext(DbContextOptionsFactory.Create(_configuration), null);
        }
    }
}
