using Microsoft.Extensions.Hosting;

namespace AspNetCore.EventSourcing.Hosting
{
    public static class Application
    {
        public static IHostBuilder CreateBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .RegisterDefaults();
        }
    }
}
