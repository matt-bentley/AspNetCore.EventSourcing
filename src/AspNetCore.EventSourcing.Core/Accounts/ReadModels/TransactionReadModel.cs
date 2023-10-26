using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;

namespace AspNetCore.EventSourcing.Core.Accounts.ReadModels
{
    public sealed class TransactionReadModel : ReadModelBase
    {
        public Guid AccountId { get; set; }
        public DateTime TransationDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string? Description { get; set; }
    }
}
