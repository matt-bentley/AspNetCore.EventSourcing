using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;

namespace AspNetCore.EventSourcing.Core.Abstractions.Guards
{
    public static partial class GuardClauseExtensions
    {
        private static void Error(string message)
        {
            throw new DomainException(message);
        }

        private static void NotFound(string message)
        {
            throw new NotFoundException(message);
        }
    }
}
