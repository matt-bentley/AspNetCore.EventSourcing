using AspNetCore.EventSourcing.Hosting;
using AspNetCore.EventSourcing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.EventSourcing.Migrations
{
    public sealed class MigrationJob : Job
    {
        private readonly BankingContext _context;

        public MigrationJob(ILogger<MigrationJob> logger,
            BankingContext context,
            IHostApplicationLifetime hostApplicationLifetime) : base(logger, hostApplicationLifetime)
        {
            _context = context;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await MigrateDatabaseAsync();
        }

        private async Task MigrateDatabaseAsync()
        {
            Logger.LogInformation("Starting database migration");
            await _context.Database.MigrateAsync();
            Logger.LogInformation("Finished database migration");
        }
    }
}
