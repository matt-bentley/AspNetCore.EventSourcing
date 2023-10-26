
namespace AspNetCore.EventSourcing.Application.Accounts.Models
{
    public sealed class AccountCreateDto
    {
        public Guid CustomerId { get; set; }
        public string? AccountNumber { get; set; }
    }
}
