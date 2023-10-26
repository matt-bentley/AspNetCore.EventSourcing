
namespace AspNetCore.EventSourcing.Application.Customers.Models
{
    public sealed class CustomerCreateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
