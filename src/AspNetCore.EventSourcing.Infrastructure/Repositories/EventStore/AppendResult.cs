
namespace AspNetCore.EventSourcing.Infrastructure.Repositories.EventStore
{
    public sealed class AppendResult
    {
        public AppendResult(long nextExpectedVersion)
        {
            NextExpectedVersion = nextExpectedVersion;
        }

        public long NextExpectedVersion { get; }
    }
}
