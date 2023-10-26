using AspNetCore.EventSourcing.Core.Customers.Entities;

namespace AspNetCore.EventSourcing.Core.Abstractions.Guards
{
    public static partial class GuardClauseExtensions
    {
        public static Customer CustomerHasLeft(this IGuardClause guardClause, Customer customer)
        {
            if (customer.HasLeft)
            {
                Error($"Customer has left: {customer.Id}");
            }
            return customer;
        }
    }
}
