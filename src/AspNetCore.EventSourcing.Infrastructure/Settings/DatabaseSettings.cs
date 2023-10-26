using Microsoft.Extensions.Configuration;

namespace AspNetCore.EventSourcing.Infrastructure.Settings
{
    public sealed class DatabaseSettings
    {
        public static DatabaseSettings Create(IConfiguration configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.GetSection("Database").Bind(databaseSettings);
            return databaseSettings;
        }

        public string? SqlConnectionString { get; set; }
    }
}
