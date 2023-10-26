
namespace AspNetCore.EventSourcing.Application.Customers.Models
{
    public sealed class CustomerDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
