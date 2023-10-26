using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;

namespace AspNetCore.EventSourcing.Core.Accounts.ReadModels
{
    public sealed class AccountReadModel : ReadModelBase
    {
        public string? AccountNumber { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OpenedDate { get; set; }
        public bool Closed { get; set; }
        public DateTime? ClosedDate { get; set; }
        public decimal Balance { get; set; }
    }
}
