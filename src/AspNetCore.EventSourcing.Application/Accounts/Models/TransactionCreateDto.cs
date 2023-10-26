
namespace AspNetCore.EventSourcing.Application.Accounts.Models
{
    public sealed class TransactionCreateDto
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
